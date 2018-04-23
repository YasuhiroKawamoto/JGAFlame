using UnityEngine;
using System;

namespace Extensions
{
	public static class MaterialEx
	{
		/// <summary>
		/// 透明度を設定します
		/// </summary>
		public static void SetAlpha(this Material self, float a)
		{
			var color = self.color;
			color.a = a;
			self.color = color;
		}
	}
}