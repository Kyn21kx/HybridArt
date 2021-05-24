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

		public static Vector operator +(Vector v, Vector u) {
			Vector w = v;
			w.x += u.x;
			w.y += u.y;
			return w;
		}

		public static Vector operator *(Vector v, float scalar) {
			Vector r = v;
			r.x *= scalar;
			r.y *= scalar;
			return r;
		}

		public static explicit operator POINT(Vector v) {
			POINT p = new POINT((int)v.x, (int)v.y);
			return p;
		}

		public override string ToString() {
			return $"({this.x}, {this.y})";
		}

	}

}
