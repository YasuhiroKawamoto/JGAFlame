using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Extensions
{
	public static class EnumEx
	{
		/// <summary>
		/// 現在のインスタンスで 1 つ以上のビット フィールドが設定されているかどうか判定
		/// </summary>
		public static bool HasFlag(this Enum self, Enum flag)
		{
			if (self.GetType() != flag.GetType())
			{
				throw new ArgumentException("flag がインスタンスの型と異なっています。");
			}

			var selfValue = Convert.ToUInt64(self);
			var flagValue = Convert.ToUInt64(flag);

			return (selfValue & flagValue) == flagValue;
		}
	}
}