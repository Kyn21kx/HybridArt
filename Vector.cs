using System;
using System.Collections.Generic;
using System.Text;

namespace InputDrawer {

	struct Vector {
		private static Vector zero = new Vector(0f, 0f);
		public static Vector Zero { get { return zero; } }

		public float x;
		public float y;

		public Vector(float x, float y) {
			this.x = x;
			this.y = y;
		}

		public override string ToString() {
			return $"({this.x}, {this.y})";
		}

	}

}
