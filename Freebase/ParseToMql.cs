using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
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

        private void HandlePrimitive(Object sender, PropertyInfo p,bool IsString)
        {
            var value = p.GetValue(sender, null);
            if (value == null)
            {
                _sb.Append("\"" + p.Name + "\"" + ":" + "null");
            }
           else if (IsString)
            {
                _sb.Append("\"" + p.Name + "\"" + ":" + "\"" + value + "\"");
            }
            else
            {
                _sb.Append("\"" + p.Name + "\"" + ":" + value);
            }
        }

        private void HandleEnumerable(IEnumerable<Object> enumerable)
        {
            var temp = enumerable;
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

        private void HandleArray(Object value)
        {
            var temp = value as Object[];
            var allnull = true;
            if (temp != null)
                if (temp.Length > 0)
                    for (var count = 0; count < temp.Length && temp[count] != null; count++)
                    {
                        allnull = false;
                        _sb.AppendLine();
                        _sb.Append("\t" + "\"" + temp[count] + "\"" + ",");
                    }
                else
                {
                    _sb.Append("\t" + "[]"+",");
                }
            if (!allnull)
            {
                _sb.Append("\t" + "[]" + ",");
                _sb.Remove(_sb.ToString().Length - 1, 1);
                _sb.Append("\t" + "}]");
            }
            else
            {
                _sb.Append("[]");
            }
        }

        private void HandleDictionary(Object data, Object value)
        {
            _sb.AppendLine();
            _sb.Append("\t\"" + data + "\"" + ":");
            if(value != null)
            TypeSwitch.Do(
                value,
                TypeSwitch.Case<Int32>(() => _sb.Append((Int32)value)),
                TypeSwitch.Case<String>(() => _sb.Append("\"" + value + "\"")),
                TypeSwitch.Case<bool>(() => _sb.Append(((bool)value).ToString().ToLower())),
                TypeSwitch.Case<Double>(() => _sb.Append((Double)value)),
                TypeSwitch.Case<float>(() => _sb.Append((Int32)value)),
                TypeSwitch.Case<Object[]>(() => HandleArray(value)),
                TypeSwitch.Case<IEnumerable<Object>>(() => HandleEnumerable(value as IEnumerable<Object>)),
                TypeSwitch.Case<Dictionary<Object, Object>>(() =>
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
                    })
                );
            else
            {
                _sb.Append("null");
            }           
            _sb.Append(",");
        }

        private void ToMqlString(Object sender)
        {
            _sb.Append("{");
            foreach (var p in _propinfo)
            {
                _sb.AppendLine();

                TypeSwitch.Do(
                    p,
                    TypeSwitch.Case<Int32>(() => HandlePrimitive(sender, p,false)),
                    TypeSwitch.Case<Double>(() => HandlePrimitive(sender, p,false)),
                    TypeSwitch.Case<float>(() => HandlePrimitive(sender, p,false)),
                    TypeSwitch.Case<bool>(() => HandlePrimitive(sender, p,false)),
                    TypeSwitch.Case<String>(() => HandlePrimitive(sender, p,true)),
                    TypeSwitch.Default(() => {                        
                        if (p.GetValue(sender, null) == null)
                            {
                                _sb.Append("\"" + p.Name + "\""+":" + "[]");
                            }
                        else
                            {
                            _sb.Append("\"" + p.Name + "\"" + ":" + "[{");
                        TypeSwitch.Do
                            (
                            p,
                            TypeSwitch.Case<Object[]>(() => HandleArray(sender,p)),
                            TypeSwitch.Case<IEnumerable<Object>>(() => HandleEnumerable(p as IEnumerable<Object>)),
                            TypeSwitch.Case<Dictionary<Object, Object>>(() =>
                                                                        {
                                                                        var myDictionary = p.GetValue(sender, null) as IDictionary<Object, Object>;
                                                                        if (myDictionary != null)
                                                                            foreach (var kvp in myDictionary)
                                                                            {
                                                                                HandleDictionary(kvp.Key, kvp.Value);
                                                                            }
                                                                        _sb.Remove(_sb.ToString().Length - 1, 1);
                                                                        _sb.AppendLine();
                                                                        _sb.Append("\t" + "}]");
                                                                        }
                                                                        ),
                            TypeSwitch.Default(
                                                () => {
                                                        _sb.Append(p.GetValue(sender, null).ToString());
                                                        _sb.Append("}" + "\"");
                                                       }
                                              )
                            );
                        }
                        })
                    );
                _sb.Append(",");
            }
            _sb.Remove(_sb.ToString().Length - 1, 1);
            _sb.AppendLine();
            _sb.Append("}");
            JsonString = _sb.ToString();
        }        
    }
    }