﻿using RestSharp;
using RestSharp.Serializers.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Exercise01 = Chapter08.Exercises.Exercise01;

namespace Chapter08
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //await Examples.GitOctokit.Demo.Run();
            //await Examples.GitHttp.Demo.Run();
            //await Examples.RestSharp.Demo.Run();
            //await Examples.Refit.Demo.Run();

            //await Activities.Activity01.Demo.Run();
            //await Activities.Activity02.Demo.Run();
            //await Activities.Activity03.Demo.Run();
            //await Activities.Activity04.Demo.Run();

            //await Exercises.Exercise01.Demo.Run();
            //await Exercises.Exercise02.Demo.Run();
            await Exercises.Exercise03.Demo.Run();
        }
    }
}
