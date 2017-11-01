using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Mothiva.Cron
{
		public sealed class ServiceSettings: ConfigurationSection
		{
				public ServiceSettings()
				{
				}

				[ConfigurationProperty( "cron", IsDefaultCollection = false )]
				public CronEntriesCollection CronEntries
				{
						get
						{
								CronEntriesCollection entriesCollection = (CronEntriesCollection)base["cron"];
								return entriesCollection;
						}
				}

				#region Cron settings
				public sealed class CronEntryElement: ConfigurationElement
				{
						public CronEntryElement()
						{
						}
						public CronEntryElement( string name )
						{
								this.Name = name;
						}

						[ConfigurationProperty( "name", IsRequired = true, IsKey = true, DefaultValue = null )]
						public string Name
						{
								get { return (string)this["name"]; }
								set { this["name"] = value; }
						}

						[ConfigurationProperty( "assembly", IsRequired = true, IsKey = false, DefaultValue = null )]
						public string Assembly
						{
								get { return (string)this["assembly"]; }
								set { this["assembly"] = value; }
						}

						[ConfigurationProperty( "schedule", IsRequired = true, IsKey = false, DefaultValue = null )]
						public CronScheduleElement Schedule
						{
								get { return (CronScheduleElement)this["schedule"]; }
								set { this["schedule"] = value; }
						}
				}
				
				public sealed class CronEntriesCollection: ConfigurationElementCollection
				{
						public CronEntriesCollection()
						{
						}

						protected override ConfigurationElement CreateNewElement()
						{
								return new CronEntryElement();
						}


						protected override ConfigurationElement CreateNewElement( string elementName )
						{
								return new CronEntryElement( elementName );
						}

						protected override Object GetElementKey( ConfigurationElement element )
						{
								return ( (CronEntryElement)element ).Name;
						}						
				}

				public sealed class CronScheduleElement: ConfigurationElement
				{
						public CronScheduleElement()
						{
						}

						[ConfigurationProperty( "minute", IsRequired = true, DefaultValue = "*" )]
						public string Minute
						{
								get { return (string)this["minute"]; }
								set { this["minute"] = value; }
						}

						[ConfigurationProperty( "hour", IsRequired = true, DefaultValue = "*" )]
						public string Hour
						{
								get { return (string)this["hour"]; }
								set { this["hour"] = value; }
						}

						/// <summary>
						/// Day of month
						/// </summary>
						[ConfigurationProperty( "dayOfMonth", IsRequired = true, DefaultValue = "*" )]
						public string DayOfMonth
						{
								get { return (string)this["dayOfMonth"]; }
								set { this["dayOfMonth"] = value; }
						}

						[ConfigurationProperty( "month", IsRequired = true, DefaultValue = "*" )]
						public string Month
						{
								get { return (string)this["month"]; }
								set { this["month"] = value; }
						}

						/// <summary>
						/// Day of Week
						/// </summary>
						[ConfigurationProperty( "dayOfWeek", IsRequired = true, DefaultValue = "*" )]
						public string DayOfWeek
						{
								get { return (string)this["dayOfWeek"]; }
								set { this["dayOfWeek"] = value; }
						}

						[ConfigurationProperty( "year", IsRequired = true, DefaultValue = "*" )]
						public string Year
						{
								get { return (string)this["year"]; }
								set { this["year"] = value; }
						}

						[ConfigurationProperty( "command", IsRequired = true, DefaultValue = null )]
						public string Command
						{
								get { return (string)this["command"]; }
								set { this["command"] = value; }
						}

						[ConfigurationProperty( "params", IsDefaultCollection = false )]
						public CronScheduleParamsCollection CommandParameters
						{
								get
								{
										CronScheduleParamsCollection entriesCollection = (CronScheduleParamsCollection)base["params"];
										return entriesCollection;
								}
						}

				}

				public sealed class CronScheduleParam: ConfigurationElement
				{
						public CronScheduleParam()
						{
						}
						public CronScheduleParam( string name )
						{
								this.Name = name;
						}

						[ConfigurationProperty( "name", IsRequired = true, IsKey = true, DefaultValue = null )]
						public string Name
						{
								get { return (string)this["name"]; }
								set { this["name"] = value; }
						}

						[ConfigurationProperty( "value", IsRequired = true, IsKey = false, DefaultValue = null )]
						public string Value
						{
								get { return (string)this["value"]; }
								set { this["value"] = value; }
						}
				}
				public sealed class CronScheduleParamsCollection: ConfigurationElementCollection
				{
						public CronScheduleParamsCollection()
						{
						}

						protected override ConfigurationElement CreateNewElement()
						{
								return new CronScheduleParam();
						}


						protected override ConfigurationElement CreateNewElement( string elementName )
						{
								return new CronScheduleParam( elementName );
						}

						protected override Object GetElementKey( ConfigurationElement element )
						{
								return ( (CronScheduleParam)element ).Name;
						}

						public Dictionary<string,string> ToDictionary()
						{
								Dictionary<string, string> dic = new Dictionary<string, string>();
								foreach ( CronScheduleParam elm in this )
										dic.Add( elm.Name, elm.Value );

								return dic;
						}
				}

				#endregion

		}
}
