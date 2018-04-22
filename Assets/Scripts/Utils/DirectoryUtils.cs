using System.IO;


namespace Util
{
	public class DirectoryUtils
	{
		/// <summary>
		/// 指定したパスにディレクトリが存在しない場合
		/// すべてのディレクトリとサブディレクトリを作成する
		/// </summary>
		public static DirectoryInfo SafeCreateDirectory(string path)
		{
			if (Directory.Exists(path))
			{
				return null;
			}
			return Directory.CreateDirectory(path);
		}
	}
}