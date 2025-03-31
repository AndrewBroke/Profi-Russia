using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Watermelon
{
    [RegisterModule("Services/Firebase Analytics")]
    public class FirebaseAnalyticsInitModule : InitModule
    {
        public FirebaseAnalyticsInitModule()
        {
            moduleName = "Firebase Analytics";
        }

        public override void CreateComponent(Initialiser Initialiser)
        {
            FirebaseAnalyticsController firebaseAnalyticsController = new FirebaseAnalyticsController();
        }
    }
}
