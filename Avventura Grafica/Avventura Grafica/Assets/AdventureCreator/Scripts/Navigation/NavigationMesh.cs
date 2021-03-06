/*
 *
 *	Adventure Creator
 *	by Chris Burton, 2013-2019
 *	
 *	"NavigationMesh.cs"
 * 
 *	This script is used by the MeshCollider and PolygonCollider
 *  navigation methods to define the pathfinding area.
 * 
 */

using UnityEngine;
using System.Collections.Generic;

namespace AC
{

	/**
	 * Defines a walkable area of AC's built-in pathfinding algorithms.
	 */
	#if !(UNITY_4_6 || UNITY_4_7 || UNITY_5_0)
	[HelpURL("https://www.adventurecreator.org/scripting-guide/class_a_c_1_1_navigation_mesh.html")]
	#endif
	public class NavigationMesh : NavMeshBase
	{

		#region Variables

		/** A List of holes within the base PolygonCollider2D */
		public List<PolygonCollider2D> polygonColliderHoles = new List<PolygonCollider2D>();
		/** The condition for which dynamic 2D pathfinding can occur by generating holes around characters (None, OnlyStationaryCharacters, AllCharacters */
		public CharacterEvasion characterEvasion = CharacterEvasion.OnlyStationaryCharacters;
		/** The number of vertices created around characters to evade (Four, Eight, Sixteen). Higher values mean greater accuracy. */
		public CharacterEvasionPoints characterEvasionPoints = CharacterEvasionPoints.Four;
		/** The scale of generated character evasion 'holes' in the NavMesh in the y-axis, relative to the x-axis. */
		public float characterEvasionYScale = 1f;
		/** A float that can be used as an accuracy parameter, should the algorithm require one */
		public float accuracy = 1f;
		/** The colour of its Gizmo when used for 2D polygons */
		public Color gizmoColour = Color.white;

		protected PolygonCollider2D[] polygonCollider2Ds;
		#if UNITY_EDITOR
		protected bool showInEditor;
		#endif

		#endregion


		#region UnityStandards

		protected void Awake ()
		{
			BaseAwake ();
		}

		#if UNITY_EDITOR

		protected void OnDrawGizmos ()
		{
			if (showInEditor)
			{
				DrawGizmos ();
			}
		}
		
		
		protected void OnDrawGizmosSelected ()
		{
			DrawGizmos ();
		}

		#endif

		#endregion


		#region PublicFunctions

		/**
		 * <summary>Integrates a PolygonCollider2D into the shape of the base PolygonCollider2D.
		 * If the shape of the new PolygonCollider2D is within the boundary of the base PolygonCollider2D, then the shape will effectively be subtracted.
		 * If the shape is instead outside the boundary of the base and overlaps, then the two shapes will effectively be combined.</summary>
		 * <param name = "newHole">The new PolygonCollider2D to integrate</param>
		 */
		public void AddHole (PolygonCollider2D newHole)
		{
			if (polygonColliderHoles.Contains (newHole))
			{
				return;
			}

			polygonColliderHoles.Add (newHole);
			ResetHoles ();

			if (GetComponent <RememberNavMesh2D>() == null)
			{
				ACDebug.LogWarning ("Changes to " + this.gameObject.name + "'s holes will not be saved because it has no RememberNavMesh2D script", gameObject);
			}
		}


		/**
		 * <summary>Removes the effects of a PolygonCollider2D on the shape of the base PolygonCollider2D.
		 * This function will only have an effect if the PolygonCollider2D was previously added, using AddHole().</sumary>
		 * <param name = "oldHole">The new PolygonCollider2D to remove</param>
		 */
		public void RemoveHole (PolygonCollider2D oldHole)
		{
			if (polygonColliderHoles.Contains (oldHole))
			{
				polygonColliderHoles.Remove (oldHole);
				ResetHoles ();
			}
		}


		/**
		 * Enables the GameObject so that it can be used in pathfinding.
		 */
		public void TurnOn ()
		{
			if (KickStarter.navigationManager.navigationEngine)
			{
				KickStarter.navigationManager.navigationEngine.TurnOn (this);
				KickStarter.navigationManager.navigationEngine.ResetHoles (this);
			}
		}
		

		/**
		 * Disables the GameObject from being used in pathfinding.
		 */
		public void TurnOff ()
		{
			if (KickStarter.settingsManager != null)
			{
				gameObject.layer = LayerMask.NameToLayer (KickStarter.settingsManager.deactivatedLayer);
			}
		}

		#endregion


		#region ProtectedFunctions

		private void ResetHoles ()
		{
			KickStarter.navigationManager.navigationEngine.ResetHoles (this);
		}

		#if UNITY_EDITOR

		protected void DrawGizmos ()
		{
			if (KickStarter.navigationManager != null)
			{
				if (KickStarter.navigationManager.navigationEngine == null) KickStarter.navigationManager.ResetEngine ();
				if (KickStarter.navigationManager.navigationEngine != null)
				{
					KickStarter.navigationManager.navigationEngine.DrawGizmos (this.gameObject);
				}
			}
		}

		#endif

		#endregion


		#region GetSet

		public PolygonCollider2D[] PolygonCollider2Ds
		{
			get
			{
				if (polygonCollider2Ds == null)
				{
					polygonCollider2Ds = GetComponents <PolygonCollider2D>();
				}
				return polygonCollider2Ds;
			}
		}

		#if UNITY_EDITOR

		/** If True, the boundary will be drawn in the Scene window (Polygon Collider-based navigation only) */
		public bool ShowInEditor
		{
			set
			{
				showInEditor = value;
			}
		}

		#endif

		#endregion

	}

}