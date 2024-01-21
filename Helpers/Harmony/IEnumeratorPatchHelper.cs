using System;
using System.Collections;

namespace ExtraLethalCompany.Helpers
{
    public class IEnumeratorPatchHelper(IEnumerator enumerator) : IEnumerable
    {
        public IEnumerator Enumerator = enumerator;
        public Action prefixAction, postfixAction;
        public Action<object> preItemAction, postItemAction;
        public Func<object, object> itemAction;

        IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
        public IEnumerator GetEnumerator()
        {
            prefixAction?.Invoke();
            while (Enumerator.MoveNext())
            {
                var item = Enumerator.Current;
                preItemAction?.Invoke(item);
                yield return itemAction?.Invoke(item);
                postItemAction?.Invoke(item);
            }
            postfixAction?.Invoke();
        }
    }
}
