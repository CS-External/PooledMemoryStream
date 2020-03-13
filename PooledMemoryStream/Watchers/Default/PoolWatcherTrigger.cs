using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PooledMemoryStreams.Watchers.Default
{
    public class PoolWatcherTrigger: IPoolWatcherTrigger
    {
        private const int CONT_INTERVAL = 1000;

        private int m_Interval;
        private Action m_TriggerAction;
        private Task m_Task;

        public PoolWatcherTrigger()
        {
            m_Interval = CONT_INTERVAL;
        }

        public PoolWatcherTrigger(int p_Interval)
        {
            m_Interval = p_Interval;
        }

        public void Start(Action p_Callback)
        {
            m_TriggerAction = p_Callback;

            if (m_TriggerAction == null)
                return;

            m_Task = new Task(ExecuteTrigger);
            m_Task.Start();
        }

        public void Stop()
        {
            m_Task = null;
            m_TriggerAction = null;
        }

        public void ExecuteTrigger()
        {
            while (m_Task != null)
            {
                Task.Delay(m_Interval).Wait();

                Action l_TriggerAction = m_TriggerAction;

                if (l_TriggerAction != null)
                    l_TriggerAction();
            }
        }
    }
}
