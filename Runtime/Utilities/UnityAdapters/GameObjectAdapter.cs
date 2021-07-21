using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace i5.Toolkit.Core.Experimental.UnityAdapters
{
    public class GameObjectAdapter : IActivateable
    {
        public GameObject Adaptee { get; private set; }

        public bool ActiveSelf
        {
            get
            {
                return Adaptee.activeSelf;
            }

            set
            {
                Adaptee.SetActive(value);
            }
        }

        public bool ActiveInHierarchy
        {
            get => Adaptee.activeInHierarchy;
            set => Adaptee.SetActive(value);
        }

        public GameObjectAdapter(GameObject adaptee)
        {
            Adaptee = adaptee;
        }
    }
}