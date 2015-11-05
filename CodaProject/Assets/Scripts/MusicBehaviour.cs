using UnityEngine;
using System.Collections;

namespace Coda {

	public class MusicBehaviour : MonoBehaviour {

		protected virtual void Awake () {

		}

		protected virtual void Start () {
			Maestro.current.Subscribe(this);
		}

		protected virtual void Update () {
			
		}

		public virtual void OnBeat () {

		}
	}

}
