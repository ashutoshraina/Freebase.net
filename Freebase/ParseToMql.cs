using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Freebase
    {
    public class ParseToMql
        {
        private readonly StringBuilder _sb;
        private readonly Type _type;
        private readonly PropertyInfo[] _propinfo;
        public String JsonString { get; private set; }

        public ParseToMql ( Object sender )
        {
            _sb = new StringBuilder();
            _type = sender.GetType();
            _propinfo = _type.GetProperties();
            JsonString = "";
            ToMqlString(sender);
        }

        private void HandleString(Object sender, PropertyInfo p)
        {
            if (p.GetValue(sender, null) == null)
            {
                _sb.Append("\"" + p.Name + "\"" + ":" + "null");
            }
            else
            {
                _sb.Append("\"" + p.Name + "\"" + ":" + "\"" + p.GetValue(sender, null) + "\"");
            }
        }

        private void HandleList(Object sender, PropertyInfo p)
        {
            var temp = p.GetValue(sender, null) as List<Object>;
            if (temp != null)
                foreach (var s in temp)
                {
                    _sb.Append("\n\t" + "\"" + s + "\"" + ",");
                }
            _sb.Remove(_sb.ToString().Length - 1, 1);
            _sb.Append("\n\t" + "}]");
        }

        private void HandlePrimitive(Object sender, PropertyInfo p)
        {
            if (p.GetValue(sender, null) == null)
            {
                _sb.Append("\"" + p.Name + "\"" + ":" + "null");
            }
            else
            {
                _sb.Append("\"" + p.Name + "\"" + ":" + p.GetValue(sender, null));
            }
        }

        private void HandleArray(Object sender, PropertyInfo p)
        {
            var temp = p.GetValue(sender, null) as Object[];
            if (temp != null)
                for (var count = 0; count < temp.Length && temp[count] != null; count++)
                {
                    _sb.AppendLine();
                    _sb.Append("\t" + "\"" + temp[count] + "\"" + ",");
                }
            _sb.Remove(_sb.ToString().Length - 1, 1);
            _sb.AppendLine();
            _sb.Append("\t" + "}]");
        }

        private void HandleEnumerable(Object value)
        {
            var temp = value as IEnumerable<Object>;
            if (temp == null) return;
            var tempEnumerator = temp.GetEnumerator();
            tempEnumerator.Reset();
            var allnull = true;
            while (tempEnumerator.MoveNext())
            {
                if (tempEnumerator.Current == null) continue;
                allnull = false;
                _sb.AppendLine();
                _sb.Append("\t\"" + tempEnumerator.Current + "\",");
            }

            if (!allnull)
            {
                _sb.Remove(_sb.ToString().Length - 1, 1);
                _sb.Append("\t}]");
            }
            else
            {
                _sb.Append("[]");
            }
        }

        private void HandleArray(Object value)
        {
            var temp = value as Object[];
            var allnull = true;
            if (temp != null)
                for (var count = 0; count < temp.Length && temp[count] != null; count++)
                {
                    allnull = false;
                    _sb.AppendLine();
                    _sb.Append("\t" + "\"" + temp[count] + "\"" + ",");
                }
            if (!allnull)
            {
                _sb.Remove(_sb.ToString().Length - 1, 1);
                _sb.Append("\t" + "}]");
            }
            else
            {
                _sb.Append("[]");
            }
        }

        private void HandleDictionary(object data, object value)
        {
            _sb.AppendLine();
            _sb.Append("\t\"" + data + "\"" + ":");
            if (value == null)
            {
                _sb.Append("null");
            }
            else if (value.GetType() == typeof (Int32))
            {
                _sb.Append((Int32) value);
            }
            else if (value.GetType() == typeof (String))
            {
                _sb.Append("\"" + value + "\"");
            }
            else if (value.GetType() == typeof (bool))
            {
                _sb.Append(((bool) value).ToString().ToLower());
            }
            else if (value.GetType() == typeof (Double))
            {
                _sb.Append((Double) value);
            }
            else if (value.GetType() == typeof (Object[]))
            {
                HandleArray(value);
            }
            else if (value.GetType() == typeof (IEnumerable<Object>))
            {
                HandleEnumerable(value);
            }
            else if (value.GetType() == typeof (Dictionary<Object, Object>))
            {
                var myDictionary = value as Dictionary<Object, Object>;
                _sb.Append("[{");
                if (myDictionary != null)
                    foreach (var kvp in myDictionary)
                    {
                        HandleDictionary(kvp.Key, kvp.Value);
                    }
                _sb.Remove(_sb.ToString().Length - 1, 1);
                _sb.AppendLine();
                _sb.Append("\t" + "}]");
            }
            _sb.Append(",");
        }

        private void ToMqlString(Object sender)
        {
            _sb.Append("{");
            foreach (var p in _propinfo)
            {
                _sb.AppendLine();
                if (p.PropertyType == typeof (Int32) || p.PropertyType == typeof (Double) ||
                    p.PropertyType == typeof (float) || p.PropertyType == typeof (bool))
                {
                    HandlePrimitive(sender, p);
                }
                else if (p.PropertyType == typeof (String))
                {
                    HandleString(sender, p);
                }
                else
                {
                    if (p.GetValue(sender, null) == null)
                    {
                        _sb.Append("\"" + p.Name + ":" + "[]");
                    }
                    else
                    {
                        _sb.Append("\"" + p.Name + "\"" + ":" + "[{");
                        if (p.PropertyType == typeof (Object[]))
                        {
                            HandleArray(sender, p);
                        }
                        else if (p.PropertyType == typeof (List<Object>))
                        {
                            HandleList(sender, p);
                        }
                        else if (p.PropertyType == typeof (Dictionary<Object, Object>))
                        {
                            var myDictionary = p.GetValue(sender, null) as Dictionary<Object, Object>;
                            if (myDictionary != null)
                                foreach (var kvp in myDictionary)
                                {
                                    HandleDictionary(kvp.Key, kvp.Value);
                                }
                            _sb.Remove(_sb.ToString().Length - 1, 1);
                            _sb.AppendLine();
                            _sb.Append("\t" + "}]");
                        }
                        else
                        {
                            _sb.Append(p.GetValue(sender, null).ToString());
                            _sb.Append("}" + "\"");
                        }
                    }
                }

                _sb.Append(",");
            }
            _sb.Remove(_sb.ToString().Length - 1, 1);
            _sb.AppendLine();
            _sb.Append("}");
            JsonString = _sb.ToString();
        }
        }
    }