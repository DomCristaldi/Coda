using UnityEngine;
using System.Collections;

namespace Coda {

	/// <summary>
	/// Subclass of Monobehaviour. Automatically subscribes to the Maestro.
	/// </summary>
	public class MusicBehaviour : MonoBehaviour {

		protected virtual void Awake () {

		}

		/// <summary>
		/// Call base.Start() in the override if you want this MusicBehaviour to auto-subscribe to the Maestro.
		/// </summary>
		protected virtual void Start () {
			Maestro.current.Subscribe(this);
		}

		protected virtual void Update () {
			
		}

		/// <summary>
		/// Called by the Maestro at Update of every beat if this MusicBehaviour is subscribed to it.
		/// </summary>
		public virtual void OnBeat () {

		}

        /// <summary>
        /// Called by the Maestro at LateUpdate of every beat if this MusicBehaviour is subscribed to it.
        /// </summary>
        public virtual void LateOnBeat () {

        }

	}

}
