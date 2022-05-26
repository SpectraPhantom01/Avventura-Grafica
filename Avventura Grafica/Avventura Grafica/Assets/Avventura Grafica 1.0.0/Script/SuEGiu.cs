using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuEGiu : MonoBehaviour
{
    public float speed = 1f;
    public float ValoreMinimo;
    public float ValoreMassimo;
    bool destinazioneRaggiunta = true;
    bool vaiVersoAlto = false;
    private void Update()
    {
        /// UI
        if (gameObject.GetComponent<RectTransform>() != null)
        {
            RectTransform rcTrasf = gameObject.GetComponent<RectTransform>();
            if (rcTrasf.position.y <= ValoreMinimo || vaiVersoAlto)
            {
                vaiVersoAlto = true;
                rcTrasf.position = new Vector3(rcTrasf.position.x, rcTrasf.position.y + speed * Time.deltaTime, rcTrasf.position.z);
                if (rcTrasf.position.y >= ValoreMassimo)
                {
                    destinazioneRaggiunta = true;
                    vaiVersoAlto = false;
                }
            }
            else if (rcTrasf.position.y >= ValoreMassimo || destinazioneRaggiunta == true)
            {
                rcTrasf.position = new Vector3(rcTrasf.position.x, rcTrasf.position.y - speed * Time.deltaTime, rcTrasf.position.z);
                if (rcTrasf.position.y <= ValoreMinimo)
                    destinazioneRaggiunta = false;
            }
        }

        /// Non UI
        
        if (transform.position.y <= ValoreMinimo || vaiVersoAlto)
        {
            vaiVersoAlto = true;
            transform.position = new Vector3(transform.position.x, transform.position.y + speed * Time.deltaTime, transform.position.z);
            if (transform.position.y >= ValoreMassimo)
            {
                destinazioneRaggiunta = true;
                vaiVersoAlto = false;
            }
        }
        else if (transform.position.y >= ValoreMassimo || destinazioneRaggiunta == true)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - speed * Time.deltaTime, transform.position.z);
            if (transform.position.y <= ValoreMinimo)
                destinazioneRaggiunta = false;
        }

        
    }
}
