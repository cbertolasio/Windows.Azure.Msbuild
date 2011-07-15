using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks
{
    public static class RhinoExtensions
    {
        /// <summary>
        /// Allows us to execute Return(with a lambda...)
        /// </summary>
        public static IMethodOptions<T> Return<T>(this IMethodOptions<T> opts, Func<T> factory)
        {

            opts.Return(default(T));    // required for Rhino.Mocks on non-void methods

            opts.WhenCalled(mi => mi.ReturnValue = factory());

            return opts;

        }

        /// <summary>
        /// Change the MockRepository back to record and then to replay, which clears all behaviour so that we may program new behavior
        /// </summary>
        public static void ClearBehavior<T>(this T fi)
        {
            fi.BackToRecord(BackToRecordOptions.All);
            fi.Replay();
        }
    }
}

