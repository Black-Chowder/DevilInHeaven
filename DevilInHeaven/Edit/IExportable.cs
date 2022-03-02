using System;
using System.Collections.Generic;
using System.Text;

namespace DevilInHeaven.Edit
{
    interface IExportable<T>
    {
        T Export();

        void Import(T e);
    }
}
