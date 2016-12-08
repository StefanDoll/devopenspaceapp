﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace open.conference.app.DataStore.Abstractions.Helpers
{
    public static class JwtClaimNames
    {
        public const string Expiration = "exp";

        public const string GivenName = "given_name";

        public const string FamilyName = "family_name";

        public const string Subject = "sub";
    }
}
