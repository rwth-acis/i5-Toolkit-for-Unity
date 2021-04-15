using System;

namespace i5.Toolkit.Core.ExperienceAPI
{
    [Serializable]
    public class Actor
    {
        public Actor() { }

        public Actor(string mail)
        {
            if (!mail.StartsWith("mailto:"))
            {
                mail = $"mailto:{mail}";
            }
            mbox = mail;
        }

        public string mbox;
    }
}
