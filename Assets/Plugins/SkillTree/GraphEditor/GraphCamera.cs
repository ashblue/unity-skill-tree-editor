using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace Adnc.SkillTree {
	public class GraphCamera {
		bool debug = false;

		public Vector2 offset; // Total offset tracked by the camera
		public bool move; // Is the camera moving?
		Vector2 prevPos; // Used to track position changes

		public float viewportSize = 6000f;

		public void BeginMove (Vector2 startPos) {
			move = true;
			prevPos = startPos;
			if (debug) Debug.Log("Begin camera move");
		}

		public void EndMove () {
			move = false;
			if (debug) Debug.Log("End camera move");
		}

		public bool PollCamera (Vector2 newPos) {
			if (!move) return false;

			offset += prevPos - newPos;
			prevPos = newPos;

//			if (debug) Debug.LogFormat("Polled offset: {0}, {1}", offset.x.ToString(), offset.y.ToString());

			return true;
		}

		// Get the mouse position considering the current camera offset
		public Vector2 GetMouseGlobal (Vector2 mouse) {
			return new Vector2(mouse.x + offset.x - (viewportSize / 2f), mouse.y + offset.y - (viewportSize / 2f));
		}

		public Vector2 GetOffsetGlobal () {
			return new Vector2(offset.x - (viewportSize / 2f), offset.y - (viewportSize / 2f));
		}

		public void Reset () {
			move = false;
			offset = new Vector2(viewportSize / 2f, viewportSize / 2f);
			if (debug) Debug.Log("Camera reset");
		}
	}
}
