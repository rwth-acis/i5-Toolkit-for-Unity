using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.ExperienceAPI
{
    [Serializable]
    public class Statement
    {
        public Actor actor;
        public Verb verb;
        public XApiObject @object;

        public Statement() { }

        public Statement(string actorMail, string verbUrl, string objectUrl)
        {
            actor = new Actor(actorMail);
            verb = new Verb()
            {
                id = verbUrl
            };
            @object = new XApiObject()
            {
                id = objectUrl
            };
        }
    }
}
