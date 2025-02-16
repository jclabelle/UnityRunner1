using System;
using System.Collections.Generic;
using WorldPersistence;

namespace Utilities
{
    public class PersistentTimer : Timer, IPersistentObject
    {
        

        // Update is called once per frame
        void Update()
        {
            base.Update();
        }

        public Dictionary<string, string> GetPersistentData()
        {
            Dictionary<string, string> pData = new Dictionary<string, string>();

            
            if(StartRealTime == DateTime.MinValue)
                pData.Add($"{nameof(StartRealTime)}", Convert.ToString(DateTime.Now));
            else
                pData.Add($"{nameof(StartRealTime)}", Convert.ToString(StartRealTime));

            pData.Add($"{nameof(StartTime)}", Convert.ToString(StartTime));
            // pData.Add($"{nameof(StartRealTime)}", Convert.ToString(StartRealTime));
            pData.Add($"{nameof(StopTime)}", Convert.ToString(StopTime));
            pData.Add($"{nameof(ElapsedTime)}", Convert.ToString(ElapsedTime));
            pData.Add($"{nameof(DurationType)}", Convert.ToString((int)DurationType));
            pData.Add($"{nameof(State)}", Convert.ToString((int)State));

            return pData;
        }

        public void SetPersistentData(Dictionary<string, string> pData)
        {
            StartTime = (float)Convert.ToDouble(pData[nameof(StartTime)]);
            StopTime = (float)Convert.ToDouble(pData[nameof(StopTime)]);
            ElapsedTime = (float)Convert.ToDouble(pData[nameof(ElapsedTime)]);
            StartRealTime = Convert.ToDateTime(pData[nameof(StartRealTime)]);
            DurationType = (ITimer.EDurationType)Convert.ToInt32(pData[nameof(DurationType)]);
            State = (ITimer.EState)Convert.ToInt32(pData[nameof(State)]);

        }
    }
}
