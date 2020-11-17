using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Gungeon;
using System.Collections;


namespace Items
{
    class ScrapGunCounter : BraveBehaviour
    {
        public static List<Gun> m_ScrapForm = new List<Gun>();


        private void Start()
        {
            Gun ScrapF1 = m_ScrapForm[0];
            Gun ScrapF2 = m_ScrapForm[1];
            Gun ScrapF3 = m_ScrapForm[2];
            Gun ScrapF4 = m_ScrapForm[3];
            Gun ScrapF5 = m_ScrapForm[4];
            Gun ScrapF6 = m_ScrapForm[5];
            Gun ScrapF7 = m_ScrapForm[6];

            Gun ScrapGold = m_ScrapForm[99];

            m_ScrapForm.Add(ScrapF1);
            m_ScrapForm.Add(ScrapF2);
            m_ScrapForm.Add(ScrapF3);
            m_ScrapForm.Add(ScrapF4);
            m_ScrapForm.Add(ScrapF5);
            m_ScrapForm.Add(ScrapF6);
            m_ScrapForm.Add(ScrapF7);
            m_ScrapForm.Add(ScrapGold);


            Gun m_CurrentGun = gameObject.GetComponent<Gun>();


        }

        private void Update()
        {

        }


        
    }


}
