using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;

namespace spa.model
{
    public abstract class PropertyBase : INotifyPropertyChanged
    {
        private Dictionary<string, PropertyValue> _propertyValues = 
            new Dictionary<string, PropertyValue>();

        private readonly Dictionary<string, Dictionary<string, PropertyValue>> _backups = 
            new Dictionary<string,Dictionary<string,PropertyValue>>();

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(T value, 
                                        bool alwaysNotify = false, 
                                            bool ignoreConditions = false,
                                                bool ignoreActions = false,
                                                    [CallerMemberName] String propertyName = null)
        {
            if (propertyName == null)
                return false;
            if (!_propertyValues.ContainsKey(propertyName))
                _propertyValues.Add(propertyName, new PropertyValue(default(T)));
            else if (!alwaysNotify && Equals(_propertyValues[propertyName], value))
                return false;

            if (!ignoreConditions)
                if (!InvokePropertyConditions(propertyName, value))
                    return false;

            _propertyValues[propertyName].Value = value;
            OnPropertyChanged(propertyName);

            if (!ignoreActions)
                InvokePropertyActions(propertyName, value);
      
            return true;
        }

        protected T GetProperty<T>([CallerMemberName] String propertyName = null)
        {
            if (propertyName == null || !_propertyValues.ContainsKey(propertyName))
                return default(T);
            return (T)_propertyValues[propertyName].Value;
        }

        public void AddPropertyCondition(string key, Func<object, bool> condition, string propertyName)
        {
            _propertyValues[propertyName].Conditions.Add(key, condition);
        }

        public void AddPropertyAction(string key, Action<object> action, string propertyName)
        {
            _propertyValues[propertyName].Actions.Add(key, action);
        }

        public void RemovePropertyCondition(string key, string propertyName)
        {
            _propertyValues[propertyName].Conditions.Remove(key);
        }

        public void RemovePropertyAction(string key, string propertyName)
        {
            _propertyValues[propertyName].Actions.Remove(key);
        }

        public bool ContainsPropertyCondition(string key, string propertyName)
        {
            return _propertyValues[propertyName].Conditions.ContainsKey(key);
        }

        public bool ContainsPropertyAction(string key, string propertyName)
        {
            return _propertyValues[propertyName].Actions.ContainsKey(key);
        }

        public bool InvokePropertyConditions(List<string> propertyNames)
        {
            var succeed = true;
            for (var i = 0; i < propertyNames.Count; i++)
                if (!InvokePropertyConditions(propertyNames[i]))
                    succeed = false;
            return succeed;
        }

        public void InvokePropertyActions(List<string> propertyNames)
        {
            for (var i = 0; i < propertyNames.Count; i++)
                InvokePropertyActions(propertyNames[i]);
        }

        public bool InvokePropertyConditions(string propertyName, object value = null)
        {
            var succeed = true;
            var propertyVal = _propertyValues[propertyName];
            value = value ?? propertyVal.Value;
            for (var i = 0; i < propertyVal.Conditions.Count; i++)
                if (!propertyVal.Conditions.Values.ToArray()[i](value))
                    succeed = false;
            return succeed;
        }

        public bool InvokePropertyConditions(string propertyName, string[] keys, object value = null)
        {
            var succeed = true;
            var propertyVal = _propertyValues[propertyName];
            value = value ?? propertyVal.Value;
            for (var i = 0; i < keys.Length; i++)
                if (propertyVal.Conditions.ContainsKey(keys[i]))
                    if (!propertyVal.Conditions[keys[i]](value))
                        succeed = false;
            return succeed;
        }

        public void InvokePropertyActions(string propertyName, object value = null)
        {
            var propertyVal = _propertyValues[propertyName];
            value = value ?? propertyVal.Value;
            for (var i = 0; i < propertyVal.Actions.Count; i++)
                propertyVal.Actions.Values.ToArray()[i](value);
        }

        public void InvokePropertyActions(string propertyName, string[] keys, object value = null)
        {
            var propertyVal = _propertyValues[propertyName];
            value = value ?? propertyVal.Value;
            for (var i = 0; i < keys.Length; i++)
                if (propertyVal.Actions.ContainsKey(keys[i]))
                    propertyVal.Actions[keys[i]](value);
        }

        public bool InvokeAllPropertyFunctions(Func<string, bool> orderSelector = null, bool reserve = false)
        {
            var result = InvokeAllPropertyConditions(orderSelector, reserve);
            InvokeAllPropertyActions(orderSelector, reserve);
            return result;
        }

        public bool InvokeAllPropertyConditions(Func<string, bool> orderSelector = null, bool reserve = false)
        {
            IEnumerable<string> keys = _propertyValues.Keys;
            if (orderSelector != null)
            {
                keys = reserve ? 
                    _propertyValues.Keys.OrderBy(orderSelector) : 
                    _propertyValues.Keys.OrderByDescending(orderSelector);
            }
            return InvokePropertyConditions(keys.ToList());
        }

        public void InvokeAllPropertyActions(Func<string, bool> orderSelector = null, bool reserve = false)
        {
            IEnumerable<string> keys = _propertyValues.Keys;
            if (orderSelector != null)
            {
                keys = reserve ?
                    _propertyValues.Keys.OrderBy(orderSelector) :
                    _propertyValues.Keys.OrderByDescending(orderSelector);
            }
            InvokePropertyActions(keys.ToList());
        }

        public void BackupProperties(string key)
        {
            _backups.Add(key, new Dictionary<string, PropertyValue>());
            foreach (var item in _propertyValues)
                _backups[key].Add(item.Key, item.Value.Clone());
        }

        public void RestoreProperties(string key)
        {
            _propertyValues = new Dictionary<string,PropertyValue>(_backups[key]);
        }

        public void RemoveBackup(string key)
        {
            _propertyValues.Remove(key);
        }

        public bool ContainsBackup(string key)
        {
            return _backups.ContainsKey(key);
        }

        public void InvokeAllPropertiesChanged()
        {
            foreach(var propertyKey in _propertyValues.Keys)
            {
                OnPropertyChanged(propertyKey);
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private class PropertyValue
        {
            public PropertyValue(object value)
            {
                Value = value;
                Conditions = new Dictionary<string,Func<object,bool>>();
                Actions = new Dictionary<string,Action<object>>();
            }

            public object Value { get; set; }
            public Dictionary<string, Func<object, bool>> Conditions { get; private set; }
            public Dictionary<string, Action<object>> Actions { get; private set; }

            public PropertyValue Clone()
            {
                var propertyVal = new PropertyValue(Value);
                propertyVal.Actions = Actions;
                propertyVal.Conditions = Conditions;
                return propertyVal;
            }
        }
    }
}
