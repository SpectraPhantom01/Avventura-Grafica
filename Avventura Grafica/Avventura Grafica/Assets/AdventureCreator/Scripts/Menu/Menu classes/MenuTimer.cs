/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2019
 *	
 *	"MenuTimer.cs"
 * 
 *	This MenuElement can be used in conjunction with MenuDialogList to create
 *	timed conversations, "Walking Dead"-style.
 * 
 */

using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AC
{

	/**
	 * A MenuElement that provides a "countdown" timer that can either show the time remaining to choose a Conversation's dialogue option or complete a QTE, or the progress made in the current QTE.
	 */
	public class MenuTimer : MenuElement
	{

		/** The Unity UI Slider this is linked to (Unity UI Menus only) */
		public Slider uiSlider;
		/** If True, then the value will be inverted, and the timer will move in the opposite direction */
		public bool doInvert;
		/** The texture of the slider bar (OnGUI Menus only) */
		public Texture2D timerTexture;
		/** What the value of the timer represents (Conversation, QuickTimeEventProgress, QuickTimeEventRemaining) */
		public AC_TimerType timerType = AC_TimerType.Conversation;
		/** The method by which this element is hidden from view when made invisible (DisableObject, DisableInteractability) */
		public UISelectableHideStyle uiSelectableHideStyle = UISelectableHideStyle.DisableObject;
		/** The amount of smoothing to apply (disabled if <= 0) */
		public float smoothingFactor = 0f;

		private LerpUtils.FloatLerp progressSmoothing = new LerpUtils.FloatLerp ();
		private float progress;
		private Rect timerRect;


		/**
		 * Initialises the MenuElement when it is created within MenuManager.
		 */
		public override void Declare ()
		{
			uiSlider = null;
			doInvert = false;
			isVisible = true;
			isClickable = false;
			timerType = AC_TimerType.Conversation;
			numSlots = 1;
			SetSize (new Vector2 (20f, 5f));
			uiSelectableHideStyle = UISelectableHideStyle.DisableObject;
			smoothingFactor = 0f;

			base.Declare ();
		}


		public override MenuElement DuplicateSelf (bool fromEditor, bool ignoreUnityUI)
		{
			MenuTimer newElement = CreateInstance <MenuTimer>();
			newElement.Declare ();
			newElement.CopyTimer (this, ignoreUnityUI);
			return newElement;
		}
		
		
		private void CopyTimer (MenuTimer _element, bool ignoreUnityUI)
		{
			if (ignoreUnityUI)
			{
				uiSlider = null;
			}
			else
			{
				uiSlider = _element.uiSlider;
			}

			doInvert = _element.doInvert;
			timerTexture = _element.timerTexture;
			timerType = _element.timerType;
			uiSelectableHideStyle = _element.uiSelectableHideStyle;
			smoothingFactor = _element.smoothingFactor;
			
			base.Copy (_element);
		}


		/**
		 * <summary>Initialises the linked Unity UI GameObject.</summary>
		 * <param name = "_menu">The element's parent Menu</param>
		 */
		public override void LoadUnityUI (AC.Menu _menu, Canvas canvas)
		{
			uiSlider = LinkUIElement <Slider> (canvas);
			if (uiSlider)
			{
				uiSlider.minValue = 0f;
				uiSlider.maxValue = 1f;
				uiSlider.wholeNumbers = false;
				uiSlider.value = 1f;
				uiSlider.interactable = false;
			}
		}


		/**
		 * <summary>Gets the boundary of the element.</summary>
		 * <param name = "_slot">Ignored by this subclass</param>
		 * <returns>The boundary Rect of the element</returns>
		 */
		public override RectTransform GetRectTransform (int _slot)
		{
			if (uiSlider)
			{
				return uiSlider.GetComponent <RectTransform>();
			}
			return null;
		}


		#if UNITY_EDITOR
		
		public override void ShowGUI (Menu menu)
		{
			string apiPrefix = "(AC.PlayerMenus.GetElementWithName (\"" + menu.title + "\", \"" + title + "\") as AC.MenuTimer)";

			MenuSource source = menu.menuSource;
			EditorGUILayout.BeginVertical ("Button");

			timerType = (AC_TimerType) CustomGUILayout.EnumPopup ("Timer type:", timerType, apiPrefix + ".timerType", "What the value of the timer represents");
			if (timerType == AC_TimerType.LoadingProgress && AdvGame.GetReferences ().settingsManager != null && !AdvGame.GetReferences ().settingsManager.useAsyncLoading)
			{
				EditorGUILayout.HelpBox ("Loading progress cannot be displayed unless asynchonised loading is enabled within the Settings Manager.", MessageType.Warning);
			}
			doInvert = CustomGUILayout.Toggle ("Invert value?", doInvert, apiPrefix + ".doInvert", "If True, then the value will be inverted, and the timer will move in the opposite direction");

			smoothingFactor = CustomGUILayout.Slider ("Value smoothing:", smoothingFactor, 0f, 1f, ".smoothingFactor", "The amount of smoothing to apply (0 = no smoothing)");

			if (source == MenuSource.AdventureCreator)
			{
				EditorGUILayout.BeginHorizontal ();
				EditorGUILayout.LabelField (new GUIContent ("Timer texture:", "The texture of the slider bar"), GUILayout.Width (145f));
				timerTexture = (Texture2D) CustomGUILayout.ObjectField <Texture2D> (timerTexture, false, GUILayout.Width (70f), GUILayout.Height (30f), apiPrefix + ".timerTexture");
				EditorGUILayout.EndHorizontal ();
			}
			else
			{
				uiSlider = LinkedUiGUI <Slider> (uiSlider, "Linked Slider:", source, "The Unity UI Slider this is linked to");
				uiSelectableHideStyle = (UISelectableHideStyle) CustomGUILayout.EnumPopup ("When invisible:", uiSelectableHideStyle, apiPrefix + ".uiSelectableHideStyle", "The method by which this element is hidden from view when made invisible");
			}
			EditorGUILayout.EndVertical ();

			if (source == MenuSource.AdventureCreator)
			{
				EndGUI (apiPrefix);
			}
		}

		#endif


		public override void OnMenuTurnOn (Menu menu)
		{
			progress = -1f;
			progressSmoothing.Reset ();

			base.OnMenuTurnOn (menu);
		}
		

		public override void PreDisplay (int _slot, int languageNumber, bool isActive)
		{
			if (Application.isPlaying)
			{
				float newProgress = GetProgress ();
				if (progress < 0f || smoothingFactor <= 0f)
				{
					progress = newProgress;
				}
				else
				{
					float lerpSpeed = (-9.5f * smoothingFactor) + 10f;
					//progress = Mathf.Lerp (progress, newProgress, Time.fixedDeltaTime * lerpSpeed);
					progress = progressSmoothing.Update (progress, newProgress, lerpSpeed);
				}

				if (doInvert)
				{
					progress = 1f - progress;
				}

				if (uiSlider)
				{
					uiSlider.value = progress;
					UpdateUISelectable (uiSlider, uiSelectableHideStyle);
				}
				else
				{
					timerRect = relativeRect;
					timerRect.width *= progress;
				}
			}
			else
			{
				timerRect = relativeRect;
			}
		}


		public override void Display (GUIStyle _style, int _slot, float zoom, bool isActive)
		{
			if (timerTexture)
			{
				GUI.DrawTexture (ZoomRect (timerRect, zoom), timerTexture, ScaleMode.StretchToFill, true, 0f);
			}
			
			base.Display (_style, _slot, zoom, isActive);
		}


		private float GetProgress ()
		{
			switch (timerType)
			{
				case AC_TimerType.Conversation:
					if (KickStarter.playerInput.activeConversation && KickStarter.playerInput.activeConversation.isTimed)
					{
						return KickStarter.playerInput.activeConversation.GetTimeRemaining ();
					}
					return 0f;

				case AC_TimerType.QuickTimeEventProgress:
					if (KickStarter.playerQTE.QTEIsActive ())
					{
						return KickStarter.playerQTE.GetProgress ();
					}
					return 0f;

				case AC_TimerType.QuickTimeEventRemaining:
					if (KickStarter.playerQTE.QTEIsActive ())
					{
						return KickStarter.playerQTE.GetRemainingTimeFactor ();
					}
					return 0f;

				case AC_TimerType.LoadingProgress:
					return KickStarter.sceneChanger.GetLoadingProgress ();
			}
			return 0f;
		}

	}

}