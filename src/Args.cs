using System;

namespace CrashTestNET
{
    public class Args
    {
        public int durationSec = 60;
        public int extraReads = 0;
        public int integMin = 10;
        public int integMax = 1000;
        public int iterDelayMin = 0;
        public int iterDelayMax = 100;
        public int readDelayMin = 0;
        public int readDelayMax = 0;
        public bool verbose;
        public bool throwaways;
        public bool autoStart;
        public bool trackMetrics;

        public Args() { }

        public bool parse(string[] argv)
        {
            for (int i = 0; i < argv.Length; i++)
            {
                string tok = argv[i];

                // start with unary flags
                     if (tok == "--verbose") verbose = true;
                else if (tok == "--start") autoStart = true;
                else if (tok == "--throwaways") throwaways = true;
                else if (tok == "--metrics") trackMetrics = true;
                else if (tok == "--help") return usage();

                // these can have argument
                else if (i + 1 < argv.Length)
                {
                    var value = argv[++i];

                         if (tok == "--duration-sec") durationSec = int.Parse(value);
                    else if (tok == "--extra-reads") extraReads = int.Parse(value);
                    else if (tok == "--integ-min") integMin = int.Parse(value);
                    else if (tok == "--integ-max") integMax= int.Parse(value);
                    else if (tok == "--iter-min") iterDelayMin = int.Parse(value);
                    else if (tok == "--iter-max") iterDelayMax= int.Parse(value);
                    else if (tok == "--read-min") readDelayMin = int.Parse(value);
                    else if (tok == "--read-max") readDelayMax= int.Parse(value);
                    else
                        return usage();
                }
                else
                    return usage();
            }
            return true;
        }

        bool usage()
        {
            Console.WriteLine(
                "Usage: CrashTestNET.exe [options]\n"
              + "\n" 
              + "Options:\n"
              + "\n"
              + "--debug        output verbose logging\n"
              + "--duration-sec test duration in seconds (default 60)\n"
              + "--extra-reads  number of random read opcodes per iteration (default 0)\n"
              + "--integ-min    minimum integration time (ms) (default 10)\n"
              + "--integ-max    maximum integration time (ms) (default 1000)\n"
              + "--iter-min     minimum iteration delay (ms) (default 0)\n"
              + "--iter-max     maximum iteration delay (ms) (default 100)\n"
              + "--metrics      track metrics\n"
              + "--read-min     minimum readout delay (ms) (default 0)\n"
              + "--read-max     maximum readout delay (ms) (default 0)\n"
              + "--start        start test on launch, then exit when done\n"
              + "--throwaways   perform throwaway acquisitions on integration time change\n"
              + "\n"
            );
            return false;
        }
    }
}
