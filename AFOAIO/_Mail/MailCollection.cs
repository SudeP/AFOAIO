using System;
using System.Collections;
using System.Collections.Generic;

namespace AFOAIO._Mail
{
    public sealed class MailCollection : ICollection, IEnumerable
    {
        internal MailCollection(Mail[] mails)
        {

        }
        public int Count => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
