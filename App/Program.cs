using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App
{
    class Program
    {
		public static string strAppPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

		public static void WriteColorLine(ConsoleColor clr, string strMsg)
		{
			Console.ForegroundColor = clr;
			Console.Write( strMsg );
			Console.ResetColor();
		}

		public static void PressKey()
		{
			WriteColorLine( ConsoleColor.White, "\nPress any key to close the program." );
			Console.ReadKey();
		}

		// Json data structure, defines how it should be setup and configured
		public struct JsonData
		{
			public string doc_name;							// Name of the documentation
			public string doc_logo;							// Path to the logo
			public Int32 doc_gen;							// Last generated
			public JObject categories;                      // Our categories
            public JObject doc_extras;

			public JsonData(string name, string logo, Int32 generated, JObject extradocs)
			{
				doc_name = name;
				doc_logo = logo;
				doc_gen = generated;
                doc_extras = extradocs;
                categories = new JObject();
			}
			public void AddToCategory(JArray array, string name)
			{
				categories[name] = array;
			}
			public void AddToCategory(JObject array, string name)
			{
				categories[name] = array;
			}
		}

		// Json Data structure setup
		public struct JsonData_Structure
		{
			private int pageindex;
			private int iIndex;
			private JObject arguments;
			private JObject jsonobject;

			public JsonData_Structure( int index, int index_page )
			{
				pageindex = index_page;
				iIndex = index;
				arguments = new JObject();
				jsonobject = new JObject();
			}
			public void AddToObject(JArray array, string name)
			{
				jsonobject[name] = array;
			}
			public void AddToObject(JObject array, string name)
			{
				jsonobject[name] = array;
			}
			public void AddToObject(string value, string name)
			{
				jsonobject[name] = value;
			}
			public void AddToObject(int value, string name)
			{
				jsonobject[name] = value;
			}
			public void AddToObject(bool value, string name)
			{
				jsonobject[name] = value;
			}
			public void AddToObject(float value, string name)
			{
				jsonobject[name] = value;
			}
			public void AddToArgs(string value, string name)
			{
				arguments[name] = value;
			}
			public void AddToArgs(bool value, string name)
			{
				arguments[name] = value;
			}
			public void AddToArgs(int value, string name)
			{
				arguments[name] = value;
			}
            public string GetName()
            {
                if ( GetValidStruct() && jsonobject.ContainsKey("name") )
                    return jsonobject["name"].ToString();
                return "";
            }
            public string GetNameSpace()
            {
                if ( GetValidStruct() && jsonobject.ContainsKey("_namespace") )
                    return jsonobject["_namespace"].ToString();
                return "";
            }
            public void AddToArgs(float value, string name)
			{
				arguments[name] = value;
			}
			public JObject GetArguments() { return arguments; }

			public int GetPageIndex() { return pageindex; }
			public int GetIndex() { return iIndex; }
			public JObject GetObject() { return jsonobject; }
			public bool GetValidStruct() { return jsonobject.Count > 0; }
		}
		public static List<JsonData_Structure> jsonStruct = new List<JsonData_Structure>();

		public static JsonData_Structure GetJsonStruct( int index, int index_page )
		{
			foreach (JsonData_Structure jsondata in jsonStruct)
			{
				if (jsondata.GetIndex() == index && jsondata.GetPageIndex() == index_page)
					return jsondata;
			}
			JsonData_Structure jdata = new JsonData_Structure( index, index_page );
			jsonStruct.Add(jdata);
			return jdata;
		}

		public static JsonData_Structure GetJsonStruct( int index_page )
		{
			foreach (JsonData_Structure jsondata in jsonStruct)
			{
				if (jsondata.GetPageIndex() == index_page)
					return jsondata;
			}
			// Invalid value
			JsonData_Structure jdata = new JsonData_Structure( -1, index_page );
			return jdata;
		}

		// Json Data Page Data
		public struct JsonData_Page
		{
			private int pageindex;
			private JArray functions;
			private JObject page;

			public JsonData_Page(int index)
			{
				pageindex = index;
				functions = new JArray();
				page = new JObject();
			}
			public void AddToPage(string value, string name)
			{
				page[name] = value;
			}
			public void AddToPage(int value, string name)
			{
				page[name] = value;
			}
			public void AddToPage(bool value, string name)
			{
				page[name] = value;
			}
			public void AddToPage(float value, string name)
			{
				page[name] = value;
			}
			public void AddToFunctions(JObject array)
			{
				functions.Add(array);
			}
			public void AddToFunctions(JArray array)
			{
				functions.Add(array);
			}
            public string GetCategoryName()
            {
                if ( GetValidStruct() )
                    return page["category"].ToString();
                return "";
            }
			public void AddFunctions()
			{
				// Add the functions last
				page["functions"] = functions;
			}

            public JObject GetObject() { return page; }
			public int GetIndex() { return pageindex; }
			public bool GetValidStruct() { return page.Count > 0; }
		}
		public static List<JsonData_Page> pageStruct = new List<JsonData_Page>();

		public static JsonData_Page GetPageStruct( int index )
		{
			foreach (JsonData_Page jsondata in pageStruct)
			{
				if (jsondata.GetIndex() == index)
					return jsondata;
			}
			JsonData_Page jdata = new JsonData_Page( index );
			pageStruct.Add(jdata);
			return jdata;
		}

		// Json Data Markdown
		public struct JsonData_Markdown
		{
			public string contents;
			public string filename;
		}

		// Json Data Structure Data
		public struct JsonData_StructureData
		{
			// Page related values (empty by default)
			public string page;
			public string subpage;
			public string category;

			// General values
			public string desc;
			public string desc_md;
			public string desc_file;

			public string infobox_enable;
			public string infobox_type;
			public string infobox_desc;

			public string type;

			// Func related values
			public string isfunc;
			public string classfunction;
			public string global;
			public string name;
			public string namefake;
            public string _base;
            public string _ref;
			public string _object;
			public string _namespace;
			public string _return;
			public string restrict;
			public string eventtype;
			public string child;
		}

		// Our config file
		public struct JsonConfig
		{
			public string name;				// Name of the doc
			public string logo;				// Logo
			public string[] paths;          // Available paths
			public string output;           // Output path
			public bool html;				// HTML output?
            public JObject extra_docs;       // Extra documentation links (external docs)
        }


		// For grabbing the comments
		public static bool bCommentGrab = false;
		public static int iPageFound = -1;
		public static int iPageIndexValue = 0;    // Page index value
		public static int iIndexValue = 0;        // Function index value
		public static int iIndexValueMD = 0;      // Markdown index value
		public static bool bPauseOnCompletion = false;
		public static bool bLogGeneration = false;

		// Markdown
		public static JsonData_Markdown mdfile = new JsonData_Markdown();
		public static List<JsonData_Markdown> jsonMD = new List<JsonData_Markdown>();
		public static List<string> JsonCategories = new List<string>();

		private static void UpdateFolderSearchArea(string foldername)
		{
			WriteColorLine( ConsoleColor.Cyan, foldername );
			WriteColorLine( ConsoleColor.White, " -- Searching...               " );
			Console.SetCursorPosition(0, Console.CursorTop);
		}

		private static string ReplaceString(string strinput, bool bHTML)
		{
			if (strinput == null)
				return "null";
			if (bHTML)
			{
				strinput = strinput.Replace("\\", "&#92;");
				strinput = strinput.Replace("/", "&#47;");
				strinput = strinput.Replace("'", "&apos;");
				strinput = strinput.Replace("\"", "&quot;");
				return strinput;
			}
			strinput = strinput.Replace("\\", "/");
			strinput = strinput.Replace("//", "/"); // 2 of the same in the same line, change to 1 instead
			return strinput;
		}

		private static void SearchFolder(string folderpath)
		{
			if (!Directory.Exists(folderpath))
			{
				WriteColorLine( ConsoleColor.Red, "Error" );
				WriteColorLine( ConsoleColor.White, ":\n" );
				WriteColorLine( ConsoleColor.Yellow, "The folder doesn't exist!\n" );
				WriteColorLine( ConsoleColor.White, "Folder:\n" );
				WriteColorLine( ConsoleColor.Red, folderpath + "\n");
				WriteToLog( "The folder path \"" + folderpath + "\" doesn't exist!" );
				return;
			}

			foreach (string file in Directory.EnumerateFiles(folderpath, "*.cpp"))
			{
                bCommentGrab = false;
				iPageFound = -1;

                //ReSortEverything();

                foreach (string line in File.ReadAllLines(file))
				{
					// Create or grab the required jsonstruct
					JsonData_Structure jsondata = GetJsonStruct(iIndexValue, iPageIndexValue);
					JsonData_Page pagedata = GetPageStruct(iPageIndexValue);
					// Start of the comment
					if (line.Contains("/**"))
					{
						mdfile.contents = "";
						mdfile.filename = "";

						if (line.Contains("PAGE"))
						{
							// Page value found
							iPageFound = 1;
						}
						else if (line.Contains("MARKDOWN"))
						{
							// Markdown value found
							iPageFound = 2;

							jsonMD.Add(mdfile);
						}
						// Json
						else if (line.Contains("JSON"))
							iPageFound = 3;

						// First comment is not the page value, ignore...
						if (iPageFound == -1)
							continue;

						bCommentGrab = true;
					}
					// Comment ended
					else if (line.Contains("*/") || line.Contains(" */"))
					{
						if (!bCommentGrab)
							continue;

						bCommentGrab = false;
						if (iPageFound == 2)
							iIndexValueMD++;
						iPageFound = -1;
						iIndexValue++;
					}
					// Let's grab our stuff!
					else if (bCommentGrab)
					{
						bool bIsComment = true;
						string buffer = line;

						// Values
						if (buffer.Contains(" * @"))
						{
							// Remove the first portion
							buffer = buffer.Replace(" * @", "");

							// Split the string
							int spaceIndex = buffer.IndexOf(" ", StringComparison.Ordinal);
							string type = buffer.Substring(0, spaceIndex);
							string value = buffer.Substring(spaceIndex + 1);

							if (iPageFound == 1)
							{
								if (type == "category")
								{
									if (JsonCategories.FindIndex(o => string.Equals(value, o, StringComparison.OrdinalIgnoreCase)) == -1)
										JsonCategories.Add(value);
								}
								pagedata.AddToPage(value, type);
							}
							else if (iPageFound == 2)
							{
								if (type == "filename")
									mdfile.filename = value;
							}
							else
							{
								if (type == "object")
									type = "_object";
								else if (type == "namespace")
									type = "_namespace";
								else if (type == "ref")
									type = "_ref";
								else if (type == "base")
									type = "_base";

								if (type == "args" || type == "arg")
								{
									string strSplit = "#";

									if (value.Contains(" # "))
										strSplit = " # ";
									else if (value.Contains(" #"))
										strSplit = " #";
									else if (value.Contains("# "))
										strSplit = "# ";

									value = value.Replace(strSplit, "#");

									string[] fields = value.Split('#');

									if (fields.Length > 1)
									{
										string comment = fields[1];
										if (comment == "")
											comment = "null";
										jsondata.AddToArgs(ReplaceString(comment, true), fields[0]);
									}
									else
										jsondata.AddToArgs("null", fields[0]);
								}
								else
								{
									if (type == "return")
										type = "_return";

									jsondata.AddToObject(value, type);
								}
							}

							bIsComment = false;
						}
						// Comments
						else
						{
							if (iPageFound != 2 && buffer.Contains(" * "))
							{
								buffer = buffer.Replace(" * ", "");
								if (iPageFound == 1)
									pagedata.AddToPage(buffer, "desc");
								else
									jsondata.AddToObject(buffer, "desc");
							}
						}

						if (iPageFound == 2)
						{
							if (bIsComment)
							{
								buffer = buffer.Replace(" * ", "");
								mdfile.contents = mdfile.contents + buffer + "\n";
							}
							jsonMD.Insert(iIndexValueMD, mdfile);
						}
					}
				}
                
                iPageIndexValue++;
			}

			DirectoryInfo directoryInfo = new DirectoryInfo(folderpath);
			if (directoryInfo.Exists)
			{
				DirectoryInfo[] subdirectoryInfo = directoryInfo.GetDirectories();
				foreach (DirectoryInfo subDirectory in subdirectoryInfo)
				{
					UpdateFolderSearchArea(subDirectory.Name);
					SearchFolder(subDirectory.FullName);
				}
			}
		}

		private static void GenerateConfig()
		{
			JsonConfig config = new JsonConfig();
			config.name = "Angelscript API Documentation";
			config.logo = "images/logo.png";
			config.paths = new string[] { "%app%/content" };
			config.output = "%app%/output";
			config.html = true;
            config.extra_docs = new JObject();
            config.extra_docs["www.angelcode.com/angelscript/sdk/docs/manual/doc_script.html"] = "Angelscript scripting manual";

			WriteColorLine( ConsoleColor.Yellow, "config.json");
			WriteColorLine( ConsoleColor.Cyan, " has been created.\n");

			File.WriteAllText( Path.Combine(strAppPath, @"config.json"), JsonConvert.SerializeObject(config, Formatting.Indented) );
		}

		private static void WriteHelpMsg( string strCmd, string desc )
		{
			WriteColorLine( ConsoleColor.White, "\t# ");
			WriteColorLine( ConsoleColor.Magenta, strCmd );
			WriteColorLine( ConsoleColor.White, "\t| ");
			WriteColorLine( ConsoleColor.Cyan, desc + "\n" );
		}

		private static void DisplayHelp()
		{
			WriteColorLine( ConsoleColor.White, "\n\t# Available commands:\n");
			WriteHelpMsg( "--log", "Logs everyhing to output.log." );
			WriteHelpMsg( "--config", "Generates a clean config.json." );
			WriteHelpMsg( "--pause", "Pauses the program after it has completed generating data.json." );
			WriteColorLine( ConsoleColor.White, "\t#\n");
		}

		public static void WriteToLog( string strMsg )
		{
			if (!bLogGeneration)
				return;

			string logfile = Path.Combine(strAppPath, @"output.log");

			using (System.IO.StreamWriter file = 
            new System.IO.StreamWriter(logfile, true))
			{
				file.WriteLine( DateTime.Now.ToString("[dddd, dd MMMM yyyy] ") + strMsg);
			}
		}

		private static void Main(string[] args)
		{
			WriteColorLine( ConsoleColor.Cyan, "Copyright © 2018 Zombie Panic! Team, All rights reserved.\n");

			// Before anything, check the args
			if (args.Length > 0)
				foreach (string arg in args)
				{
					if (arg.Contains("--help"))
					{
						DisplayHelp();
						PressKey();
						return;
					}
					else if (arg.Contains("--pause"))
					{
						bPauseOnCompletion = true;
					}
					else if (arg.Contains("--log"))
					{
						bLogGeneration = true;
					}
					else if (arg.Contains("--config") || arg.Contains("--conf"))
					{
						GenerateConfig();
						if (bPauseOnCompletion)
							PressKey();
						return;
					}
				}

			Console.WriteLine("");

			WriteColorLine( ConsoleColor.White, "Angelscript API Generator written by:\n");
			WriteColorLine( ConsoleColor.Cyan, "Johan \"JonnyBoy0719\" Ehrendahl\n");
			WriteColorLine( ConsoleColor.Cyan, "Joël \"Shepard62FR\" Troch\n");

			Console.WriteLine("\n");

			List<JObject> pages = new List<JObject>();

			// Remote file reading
			string strRemoteFile = Path.Combine(strAppPath, @"config.json");

			// Default setup
			JsonConfig config = new JsonConfig();
			config.name = "Angelscript API Documentation";
			config.logo = "images/logo.png";
			config.paths = new string[] { "%app%/content" };
			config.output = "%app%/output";
            config.extra_docs = new JObject();

            // Read our config file
            if ( File.Exists( strRemoteFile ) )
				config = JsonConvert.DeserializeObject<JsonConfig>( ReplaceString( File.ReadAllText( strRemoteFile ), false ) );
			else
				GenerateConfig();

			// Replace %app%
			string strOutputPath = config.output.Replace( "%app%", strAppPath );

			// Generate our time
			Int32 doc_gen = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

			// Init our data
			JsonData data = new JsonData(config.name, config.logo, doc_gen, config.extra_docs);

			bCommentGrab = false;
			iPageFound = -1;
			iPageIndexValue = 0;    // Page index value
			iIndexValue = 0;        // Function index value
			iIndexValueMD = 0;      // Markdown index value

			WriteToLog( "Starting Angelscript API generation..." );

			// Check our folders, and read the file
			foreach (string path in config.paths)
			{
				// Replace %app%
				string folderPath = path.Replace( "%app%", strAppPath );
				SearchFolder( ReplaceString( folderPath, false ) );
			}

            // Let's sort everything
            JsonCategories.Sort();

            // Log what we generated!
            {
				WriteColorLine(ConsoleColor.White, "Preparing to generate ");

				int iTempValue = 0;
				foreach (JsonData_Page pagedata in pageStruct)
				{
					if (!pagedata.GetValidStruct())
						continue;
					iTempValue++;
				}

				WriteColorLine(ConsoleColor.Magenta, iTempValue + " ");
				WriteColorLine(ConsoleColor.White, "page(s) and ");

				int iTempValue2 = 0;
				foreach (JsonData_Structure jsondata in jsonStruct)
				{
					if (!jsondata.GetValidStruct())
						continue;
					iTempValue2++;
				}

				WriteColorLine(ConsoleColor.Magenta, iTempValue2 + " ");
				WriteColorLine(ConsoleColor.White, "function(s)\n\n");

				WriteToLog( "Preparing to generate " + iTempValue + " page(s) and " + iTempValue2 + " function(s)" );
			}

            WriteToLog( "================================================");

			Thread.Sleep( 1200 );

            // First, we sort the pages, so they are in the correct places on the menu
            pageStruct.Sort((x, y) => string.Compare(x.GetCategoryName(), y.GetCategoryName()));

            // After that, we sort all our functions, so that are sorted by name.
            jsonStruct.Sort((x, y) => string.Compare(x.GetName(), y.GetName()));

            // Now, sort namespaces, because we don't want to jumble them up with non name spaces.
            //jsonStruct.Sort((x, y) => string.Compare(x.GetNameSpace(), y.GetNameSpace()));

            foreach (string category in JsonCategories)
			{
				JObject classpage = new JObject();
				classpage.RemoveAll();

                // Sort the pages
                /*
                pageStruct.Sort(delegate (JsonData_Page x, JsonData_Page y)
                {
                    JsonData_StructureData data_x_compare = JsonConvert.DeserializeObject<JsonData_StructureData>(x.GetObject().ToString());
                    JsonData_StructureData data_y_compare = JsonConvert.DeserializeObject<JsonData_StructureData>(y.GetObject().ToString());
                    if (data_x_compare.category == null && data_y_compare.category == null) return 0;
                    else if (data_x_compare.category == null) return -1;
                    else if (data_y_compare.category == null) return 1;
                    else return data_x_compare.category.CompareTo(data_x_compare.category);
                });
                */

                // Go trouch each page
                foreach (JsonData_Page pagedata in pageStruct)
				{
					if (!pagedata.GetValidStruct())
						continue;

                    // Temp info
                    JsonData_StructureData pagedata_temp = JsonConvert.DeserializeObject<JsonData_StructureData>(pagedata.GetObject().ToString());

					// Make sure it's the same category!
					if (pagedata_temp.category != category)
						continue;

                    // Sort the json struct
                    /*
                    jsonStruct.Sort(delegate (JsonData_Structure x, JsonData_Structure y)
                    {
                        JsonData_StructureData data_x_compare = JsonConvert.DeserializeObject<JsonData_StructureData>(x.GetObject().ToString());
                        JsonData_StructureData data_y_compare = JsonConvert.DeserializeObject<JsonData_StructureData>(y.GetObject().ToString());
                        if (data_x_compare.name == null && data_y_compare.name == null) return 0;
                        else if (data_x_compare.name == null) return -1;
                        else if (data_y_compare.name == null) return 1;
                        else return data_x_compare.name.CompareTo(data_x_compare.name);
                    });
                    */

                    // Grab all the functions
                    foreach (JsonData_Structure jsondata in jsonStruct)
					{
						// Make sure the page index is the same!
						if (jsondata.GetPageIndex() != pagedata.GetIndex())
							continue;

						// Not a valid struct?
						if (!jsondata.GetValidStruct())
							continue;

						JsonData_StructureData dataconfig = JsonConvert.DeserializeObject<JsonData_StructureData>(jsondata.GetObject().ToString());

						JObject function = new JObject();
						function["global"] = dataconfig.global;
						function["name"] = dataconfig.name;
						function["namefake"] = dataconfig.namefake;
						function["isfunc"] = dataconfig.isfunc;
						function["classfunction"] = dataconfig.classfunction;
                        function["base"] = dataconfig._base;
                        function["object"] = dataconfig._object;
						function["ref"] = dataconfig._ref;
						function["namespace"] = dataconfig._namespace;
						function["child"] = dataconfig.child;

						function["desc"] = ReplaceString( dataconfig.desc, true );
						function["desc_md"] = dataconfig.desc_md;
						function["desc_file"] = dataconfig.desc_file;
						function["type"] = dataconfig.type;

						function["restrict"] = dataconfig.restrict;
						function["eventtype"] = dataconfig.eventtype;

						function["infobox_enable"] = dataconfig.infobox_enable;
						function["infobox_type"] = dataconfig.infobox_type;
						function["infobox_desc"] = ReplaceString( dataconfig.infobox_desc, true );

						function["return"] = dataconfig._return;

						// Add arguments
						function["args"] = jsondata.GetArguments();

						pagedata.AddToFunctions( function );

						// Log what we generated!
						WriteColorLine( ConsoleColor.White, "Generated function \"");
						WriteColorLine( ConsoleColor.Yellow, dataconfig.name);
						WriteColorLine( ConsoleColor.White, "\"!\n\n");

						WriteToLog( "Generated function: " + dataconfig.name );

				//		if ( config.html )
				//			GenerateHTML( pagedataconfig.page, function );
					}

					// Add the functions
					pagedata.AddFunctions();

					// Add to class page
					JsonData_StructureData pagedataconfig = JsonConvert.DeserializeObject<JsonData_StructureData>(pagedata.GetObject().ToString());
					classpage[pagedataconfig.page] = pagedata.GetObject();

					// Add to category
					data.AddToCategory(classpage, category);

					// Log what we generated!
					WriteColorLine( ConsoleColor.White, "Generated page \"");
					WriteColorLine( ConsoleColor.Green, pagedataconfig.page);
					WriteColorLine( ConsoleColor.White, "\"!\n\n");

                    WriteToLog( "Generated page \"" + pagedataconfig.page + "\"");
                    WriteToLog( "================================================");

				//	if ( config.html )
				//		GenerateHTML( pagedataconfig.page, function );
				}
			}

			// Create our json
			string json = JsonConvert.SerializeObject(data);

			// Write directory
			Directory.CreateDirectory( strOutputPath );

			// Only create this folder if we actually have markdown related files that needs to be created
			if (jsonMD.Count > 0)
				Directory.CreateDirectory( Path.Combine(strOutputPath, @"markdown") );

			// Write markdown files
			foreach (JsonData_Markdown MDfile in jsonMD)
			{
				if (MDfile.filename != "")
					File.WriteAllText( Path.Combine( strOutputPath, @"markdown/" + MDfile.filename.ToLower() + ".md" ), MDfile.contents );
			}

			// Write our data
			File.WriteAllText( Path.Combine(strOutputPath, @"data.json"), json );

			WriteColorLine( ConsoleColor.White, "\"");
			WriteColorLine( ConsoleColor.Green, config.name);
			WriteColorLine( ConsoleColor.White, "\" has been generated!\n\n");

			WriteColorLine( ConsoleColor.Yellow, "data.json");
			WriteColorLine( ConsoleColor.White, " can be found at @ \"");
			WriteColorLine( ConsoleColor.Green, strOutputPath);
			WriteColorLine( ConsoleColor.White, "\"\n");

			WriteToLog( "\"" + config.name + "\" has been generated! data.json can be found at \"" + strOutputPath + "\"");

			if (bPauseOnCompletion)
				PressKey();
        }
    }
}
