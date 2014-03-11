using System;
using System.Collections.Generic;
using System.Text;

namespace Freebase
{
	public class ParseToMql {
		private readonly StringBuilder _sb;
		private static IDictionary<string, object> _propertyValues;

		public String JsonString {   
			get { return _sb.ToString(); } 
			private set { value = ""; } 
		}

		public ParseToMql (dynamic d) {
			_sb = new StringBuilder();
			_propertyValues = (IDictionary<string, object>)d;  
			ToMqlString();                    
		}

		private void HandlePrimitive (String key, Object value, bool isString) {
			if ( value == null ) {
				_sb.Append("\"" + key + "\"" + ":" + "null");
			} else if ( isString ) {
				_sb.Append("\"" + key + "\"" + ":" + "\"" + value + "\"");
			} else {
				_sb.Append("\"" + key + "\"" + ":" + value);
			}
		}

		private void HandleEnumerable (IEnumerable<Object> enumerable) {
			_sb.Append("[{");
			var temp = enumerable;
            
			if ( temp == null )
				return;
           
			var tempEnumerator = temp.GetEnumerator();
			tempEnumerator.Reset();
			var allnull = true;
          
			while (tempEnumerator.MoveNext()) {
				if ( tempEnumerator.Current == null )
					continue;
				allnull = false;
				_sb.AppendLine();
				_sb.Append("\t\"" + tempEnumerator.Current + "\",");
			}
          
			if ( !allnull ) {
				_sb.Remove(_sb.ToString().Length - 1, 1);
				_sb.Append("\t}]");
			} else {
				_sb.Append("[]");
			}
		}

		private void HandleArray (Object[] value) {
			var allnull = true;
			_sb.Append("[{");
			for (var count = 0; count < value.Length && value[count] != null; count++) {
				allnull = false;
				_sb.AppendLine();
				_sb.Append("\t" + "\"" + value[count] + "\"" + ",");
			}         
			if ( allnull ) {
				_sb.Append("}]");
			} else {
				_sb.Append("[]");
			}
		}

		private void HandleDictionary (Object key,Object value) {
			_sb.AppendLine();
			_sb.Append("\t\"" + key + "\"" + ":");
			if ( value != null ) {
				TypeSwitch.Do(value, TypeSwitch.Case<Int32>(
                    () => _sb.Append((Int32)value)), 
                    TypeSwitch.Case<String>(() => _sb.Append("\"" + value + "\"")), 
                    TypeSwitch.Case<bool>(() => _sb.Append(((bool)value).ToString().ToLower())), 
                    TypeSwitch.Case<Double>(() => _sb.Append((Double)value)), 
                    TypeSwitch.Case<float>(() => _sb.Append((Int32)value)), 
                    TypeSwitch.Case<Object[]>(() => HandleArray(value as Object[])), 
                    TypeSwitch.Case<IEnumerable<Object>>(() => HandleEnumerable(value as IEnumerable<Object>)), 
                    TypeSwitch.Case<Dictionary<Object, Object>>(() => {
					var innerDictionary = value as Dictionary<Object, Object>;
					_sb.Append("[{");
					if ( innerDictionary != null )
						foreach (var kvp in innerDictionary) {
							HandleDictionary(kvp.Key, kvp.Value);
						}
					_sb.Remove(_sb.ToString().Length - 1, 1);
					_sb.AppendLine();
					_sb.Append("\t" + "}]");
				}));
			} else {
				_sb.Append("null");
			}           
			_sb.Append(",");
		}

		private void ToMqlString () {
			_sb.Append("[{");
			foreach (var kvp in _propertyValues) {
				var key = kvp.Key;
				var value = kvp.Value;
				_sb.AppendLine();
				if ( value == null ) {
					_sb.Append("\"" + key + "\"" + ":" + "null");                    
				} else {
					TypeSwitch.Do(
						value,
						TypeSwitch.Case<Int32>(() => HandlePrimitive(key, value, false)),
						TypeSwitch.Case<double>(() => HandlePrimitive(key, value, false)),
						TypeSwitch.Case<float>(() => HandlePrimitive(key, value, false)),
						TypeSwitch.Case<bool>(() => HandlePrimitive(key, value, false)),
						TypeSwitch.Case<string>(() => HandlePrimitive(key, value, true)),
						TypeSwitch.Default(() => {                           
							_sb.Append("\"" + key + "\"" + ":");
							TypeSwitch.Do
                                    (
								value,
								TypeSwitch.Case<Object[]>(() => HandleArray(value as Object[])),
								TypeSwitch.Case<IEnumerable<Object>>(() => HandleEnumerable(value as IEnumerable<Object>)),
								TypeSwitch.Case<Dictionary<Object, Object>>(() => {
									_sb.Append("[{");
									var innerDictionary = value as Dictionary<Object, Object>;
									if ( innerDictionary != null )
										foreach (var innerkvp in innerDictionary) {
											HandleDictionary(innerkvp.Key, innerkvp.Value);
										}
									_sb.Remove(_sb.ToString().Length - 1, 1);
									_sb.AppendLine();
									_sb.Append("\t" + "}]");
								}
								),
								TypeSwitch.Default(
									() => {
										_sb.Append(value.ToString());
										_sb.Append("}" + "\"");
									}
								)
							);
						})
					);
				}
				_sb.Append(",");
			}
			_sb.Remove(_sb.ToString().Length - 1, 1);
			_sb.AppendLine();
			_sb.Append("}]");
			JsonString = _sb.ToString();
		}
	}
}