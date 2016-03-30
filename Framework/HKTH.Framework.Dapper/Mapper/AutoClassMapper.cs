using System;
using HZMall.Framework.Dapper.Mapper;

namespace HZMall.Framework.Dapper.Mapper
{
	/// <summary>
	///     Automatically maps an entity to a table using a combination of reflection and naming conventions for keys.
	/// </summary>
	public class AutoClassMapper<T> : ClassMapper<T> where T : class
	{
		public AutoClassMapper()
		{
			Type type = typeof (T);
			Table(type.Name);
			AutoMap();
		}
	}
}