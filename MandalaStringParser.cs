using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MandalaGenerator
{
    public class ParserDictionaries
    {  
    
         public Dictionary<string,Type> DictonaryEnumParser ;
        public Dictionary<string ,string> DictonaryStringParser ;

        public ParserDictionaries()
        {
                
            
            DictonaryEnumParser = new Dictionary<string,Type>();
            DictonaryEnumParser.Add("anima", typeof(TypeOfMandalaGeneretionalProgress));
            DictonaryEnumParser.Add("mov", typeof(TypeOfMandalaMovement));
            DictonaryEnumParser.Add("evo", typeof(TypeOfMandalaGeneretionalProgress));
            DictonaryEnumParser.Add("schema", typeof(TypeOfSchemaAnimation));

            DictonaryStringParser = new Dictionary<string ,string>();

            DictonaryStringParser.Add("childs", "ChildsCounter");
            DictonaryStringParser.Add("gens", "Generations");
            DictonaryStringParser.Add("start", "PatternCounter"); 

           

        }

    }

  public  class MandalaOptions 
    {
        public TypeOfMusicAnimation TypeOfMusicAnimation { get; set; } = TypeOfMusicAnimation.Normal;
        public TypeOfMandalaGeneretionalProgress generetionalProgress { get; set; } = TypeOfMandalaGeneretionalProgress.Const;
        public TypeOfMandalaMovement movementPattern { get; set; } = TypeOfMandalaMovement.Const;
        public TypeOfSchemaAnimation schemaAnimation { get; set; } = TypeOfSchemaAnimation.Single;

        public int PatternCounter { get; set; } = 8;
        public int ChildsCounter { get; set; } = 3;
        public int Generations { get; set; } = 5; 

        public MandalaOptions() { }

    
    }
    public class MandalaStringParser
    {
        ParserDictionaries ParserDictionaries = new ParserDictionaries();   
        public MandalaStringParser() { }

       public MandalaOptions GenerteOption(string toParse) 
        {
            
            string[] mandalaProps = toParse.Split(',');
            MandalaOptions options = new MandalaOptions();
            PropertyInfo[] infos = options.GetType().GetProperties();

            for (int j = 0; j < mandalaProps.Length; j++)
            {
                string[] parts = mandalaProps[j].Split(':');

                string prop = parts[0].Trim();

                int value;
                Int32.TryParse(parts[1].Trim(), out value);

                Type type;
                string nameOfProperty;

                if (ParserDictionaries.DictonaryEnumParser.TryGetValue(prop, out type))
                {
                    foreach (PropertyInfo info in infos)
                    {
                        if (info.PropertyType == type)
                        {
                            object enumValue = Enum.ToObject(type, value);
                            info.SetValue(options, enumValue);
                            break;
                        }
                    }
                }
                else if (ParserDictionaries.DictonaryStringParser.TryGetValue(prop, out nameOfProperty))
                {
                    foreach (PropertyInfo info in infos)
                    {
                        if (info.Name == nameOfProperty)
                        {
                            info.SetValue(options, value);
                            break;
                        }
                    }
                }
            }
            return options; 

        }
       public MandalaOptions[] GenerateMandalaFromCode(string MandalaCode)
        {
            MandalaCode.Trim('\r');
            string[] Mandalas = MandalaCode.Split('|').Where(x => x!="").ToArray();

            List<MandalaOptions> sequence = new List<MandalaOptions>();


            for (int i = 0; i < Mandalas.Length; i++)
            {
                        if (Mandalas[i].ToLower().Contains("for"))
                        {
                            int looplen = Int32.Parse(Mandalas[i].Split(')', '(')[1]);
                            i++;
                            for (int j = 0; j < looplen; j++)
                            { sequence.Add(GenerteOption(Mandalas[i])); }
                        }
                        else
                        {

                            sequence.Add( GenerteOption(Mandalas[i]));
                        }

             }

                  return sequence.ToArray();

            } 
         
        }

        // | definisce frame 
    }

