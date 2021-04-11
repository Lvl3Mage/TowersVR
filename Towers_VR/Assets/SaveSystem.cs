using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
	public static void SaveTeamConfigs(BaseTeam[] baseTeams, string filename){
		BinaryFormatter formatter = new BinaryFormatter();
		string filepath = CreateTeamConfigPath(filename);
		FileStream stream = new FileStream(filepath, FileMode.Create);

		TeamConfigData TeamConfigs = new TeamConfigData(baseTeams);

		formatter.Serialize(stream, TeamConfigs);
		stream.Close();
	}

	public static BaseTeam[] LoadTeamConfigs(string filename){
		string filepath = CreateTeamConfigPath(filename);
		if(File.Exists(filepath)){
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(filepath, FileMode.Open);

			TeamConfigData TeamConfigs = formatter.Deserialize(stream) as TeamConfigData;
			stream.Close();

			BaseTeam[] baseTeams = TeamConfigs.ExtractBaseTeams();
			return baseTeams;
		}
		else{
			Debug.LogError("Save file with name " + filename + " found");
			return null;
		}
	}


	static string CreateTeamConfigPath(string filename){
		CheckDirectory(Application.persistentDataPath +"/Team Configs");
		return Application.persistentDataPath +"/Team Configs/"+ filename + ".teamconfigs";
	}
	static void CheckDirectory(string directoryPath){
		if(!Directory.Exists(directoryPath))
		{
		    Directory.CreateDirectory(directoryPath);
		 
		}
	}
}
