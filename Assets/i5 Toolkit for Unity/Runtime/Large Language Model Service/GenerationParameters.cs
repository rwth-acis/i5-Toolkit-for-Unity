using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i5.Toolkit.Core.LargeLanguageModels
{
	public class GenerationParameters
	{
		public string genkey = "i5 Toolkit for Unity";
		public int max_context_length = 4096;
		public int max_length = 200;
		public string prompt;
		public double rep_pen = 1.1;
		public int rep_pen_range = 320;
		public int[] sampler_order = { 6, 0, 1, 3, 4, 2, 5 };
		public int sampler_seed;
		public string[] stop_sequence = new string[] { "\n\n\n", "\t\t\t\t\t\t\t\t", "\n\n \n\n \n\n" };
		public double temperature = 0.7;
		public double tfs = 1;
		public double top_a = 0;
		public int top_k = 100;
		public double top_p = 0.92;
		public double min_p = 0;
		public double typical = 1;
		public bool use_default_badwordsids = false;
		public double mirostat = 0;
		public double mirostat_tau = 5;
		public double mirostat_eta = 0.1;
		public string grammar;
		public bool grammar_retain_state = false;

		private readonly Random random = new Random();

		public static GenerationParameters Deterministic
		{
			get => new GenerationParameters()
			{
				temperature = 0,
				rep_pen = 1.18,
				rep_pen_range = 2048,
				top_p = 0,
				top_a = 0,
				top_k = 1,
				typical = 1,
				tfs = 1,
				sampler_order = new int[] { 6, 0, 1, 3, 4, 2, 5},
				mirostat = 0,
				mirostat_tau = 5,
				mirostat_eta = 0.1
			};
		}

		public static GenerationParameters SimpleProxyForTavern
		{
			get => new GenerationParameters
			{
				temperature = 0.65,
				rep_pen = 1.18,
				rep_pen_range = 2048,
				top_p = 0.47,
				top_a = 0,
				top_k = 42,
				typical = 1,
				tfs = 1,
				sampler_order = new int[] { 6, 0, 1, 3, 4, 2, 5 },
				mirostat = 0,
				mirostat_tau = 5,
				mirostat_eta = 0.1
			};
		}

		public static GenerationParameters Storywriter
		{
			get => new GenerationParameters
			{
				temperature = 0.72,
				rep_pen = 1.1,
				rep_pen_range = 2048,
				top_p = 0.73,
				top_a = 0,
				top_k = 0,
				typical = 1,
				tfs = 1,
				sampler_order = new int[] { 6, 5, 0, 2, 3, 1, 4 },
				mirostat = 0,
				mirostat_tau = 5,
				mirostat_eta = 0.1
			};
		}

		public static GenerationParameters BestGuess
		{
			get => new GenerationParameters
			{
				temperature = 0.8,
				rep_pen = 1.15,
				rep_pen_range = 2048,
				top_p = 0.9,
				top_a = 0,
				top_k = 100,
				typical = 1,
				tfs = 1,
				sampler_order = new int[] { 6, 5, 0, 2, 3, 1, 4 },
				mirostat = 0,
				mirostat_tau = 5,
				mirostat_eta = 0.1
			};
		}

		public GenerationParameters() : this("", -1)
		{
		}

		public GenerationParameters(string prompt, int seed = -1)
		{
			if (seed < 0)
			{
				sampler_seed = random.Next(1, 999999);
			}
			else
			{
				sampler_seed = seed;
			}

			this.prompt = prompt;
		}
	}
}
