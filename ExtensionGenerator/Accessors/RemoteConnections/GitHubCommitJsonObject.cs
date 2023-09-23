using System;
using System.Collections.Generic;

namespace ExtensionGenerator.Accessors.RemoteConnections
{

    using System;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class Commit
    {
        public string url { get; set; }
        public string sha { get; set; }
        public string node_id { get; set; }
        public string html_url { get; set; }
        public GitUser author { get; set; }
        public GitUser committer { get; set; }
        public string message { get; set; }
        public int comment_count { get; set; }
        public Tree tree { get; set; }
    }

    public class GitUser
    {
        public string name { get; set; }
        public string email { get; set; }
        public string date { get; set; }
    }

    public class Tree
    {
        public string sha { get; set; }
        public string url { get; set; }
    }


}
