using System;
using System.Collections.Generic;

namespace FreebaseTests
    {
    public class Question
        {
            public String type { get; set; }
            public String name { get; set; }
            public Dictionary<Object, Object> album { get; set; }
            public Question()
            {
                album = new Dictionary<Object, Object>();
            }
        }
    }