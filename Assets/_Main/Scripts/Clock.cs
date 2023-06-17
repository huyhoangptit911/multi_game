

using UnityEngine;
using UnityEngine.Assertions;

namespace Chessticle
{
    public class Clock
    {
        public Clock(int timeSeconds)
        {
            m_WhiteTime = timeSeconds;
            m_BlackTime = timeSeconds;
        }

        public void SwitchPlayer(double serverTime)
        {
            if (!m_Running)
            {
                return;
            }

            switch (m_CurrentPlayer)
            {
                case Color.None:
                    m_CurrentPlayer = Color.Black;
                    break;
                case Color.White:
                    m_WhiteTime -= serverTime - m_LastSwitchServerTime;
                    m_CurrentPlayer = Color.Black;
                    break;
                case Color.Black:
                    m_BlackTime -= serverTime - m_LastSwitchServerTime;
                    m_CurrentPlayer = Color.White;
                    break;
            }

            if (m_WhiteTime < 0)
            {
                m_WhiteTime = 0d;
            }

            if (m_BlackTime < 0)
            {
                m_BlackTime = 0d;
            }

            m_LastSwitchServerTime = serverTime;
        }

        public float GetTime(Color color, double serverTime)
        {
            Assert.IsTrue(color != Color.None);

            float delta = 0;
            if (m_Running && m_CurrentPlayer == color)
            {
                delta = (float) (serverTime - m_LastSwitchServerTime);
            }

            var time = color == Color.White ? m_WhiteTime : m_BlackTime;
            var value = Mathf.Max(0, (float) (time - delta));
            return value;
        }

        public void Stop()
        {
            m_Running = false;
        }

        Color m_CurrentPlayer = Color.None;
        bool m_Running = true;
        double m_LastSwitchServerTime;
        double m_WhiteTime;
        double m_BlackTime;
    }
}