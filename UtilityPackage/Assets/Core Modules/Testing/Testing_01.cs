using FickleFrames.Systems;
using System.Collections;
using UnityEngine;
using TMPro;

namespace Assets.Core_Modules.Testing
{
    public class Testing_01 : MonoBehaviour
    {
        public TMP_Text text;

        private int seconds = 0;
        private int minutes = 0;


        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.D))
                ChronoSystem.CreateNewTimer("testTimer");
            if (Input.GetKeyDown(KeyCode.F))
                ChronoSystem.PauseTimer("testTimer");
            if (Input.GetKeyDown(KeyCode.G))
                ChronoSystem.ResumeTimer("testTimer");

            text.text = showTime(Mathf.RoundToInt(ChronoSystem.GetElapsedTime("testTimer") * 1000f));
        }

        // Update is called once per frame
        string showTime(int milliseconds)
        {
            seconds = milliseconds / 1000;
            minutes = seconds / 60;

            milliseconds = milliseconds % 1000;
            seconds = seconds % 60;

            return $"{minutes} : {seconds} : {milliseconds}";
        }
    }
}