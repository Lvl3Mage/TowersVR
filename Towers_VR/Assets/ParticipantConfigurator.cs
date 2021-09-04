using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ParticipantConfigurator : MonoBehaviour
{
    public class ConfigurableTeam
    {
        string name;
        Color color;
        ParticipantConfigurationCard[] participants;
        public ConfigurableTeam(BaseTeam team, ParticipantConfigurationCard[] newParticipants){
            name = team.teamName;
            color = team.teamColor;
            participants = newParticipants;
        }
        public BaseTeam GetTeam(){
            BaseTeam team = new BaseTeam(name, color);
            team.SetParticipants(GetBaseParticipants());
            return team;
        }
        BaseParticipant[] GetBaseParticipants(){
            BaseParticipant[] baseParticipants = new BaseParticipant[participants.Length];
            for(int i = 0; i < participants.Length; i++){
                baseParticipants[i] = participants[i].GetParticipant();
            }
            return baseParticipants;
        }
    }
    [SerializeField] Camera RenderCam;
    [SerializeField] GameObject Canvas;
    [SerializeField] Object ParticipantCard;
    [SerializeField] Transform CardParent;
    [SerializeField] ConfiguredParticipant DefaultConfiguration;
    GameStarter GameStarter;
    ConfigurableTeam[] teams;
    void Start(){
        // disabling the camera at start
        ToggleRendering(false);
        GameStarter = GameObject.FindGameObjectWithTag("GameStarter").GetComponent<GameStarter>();
        if(GameStarter == null){
            Debug.LogError("No GameStarter Found in scene");
        }
    }
    public void ConfigureParticipants(BaseTeam[] baseTeams){
        // enabling the camera when the switch happens
        ToggleRendering(true);
        teams = new ConfigurableTeam[baseTeams.Length];
        
        for(int i = 0; i < baseTeams.Length; i++){
            
            BaseParticipant[] participants = baseTeams[i].participants;
            
            ParticipantConfigurationCard[] Cards = new ParticipantConfigurationCard[participants.Length];

            for(int j = 0; j < participants.Length; j++){
                Cards[j] = (Instantiate(ParticipantCard, CardParent) as GameObject).GetComponent<ParticipantConfigurationCard>();
                //in case the configuration has not been set
                if(participants[j].participantConfiguration == null){
                    participants[j].participantConfiguration = DefaultConfiguration; // set it to the default one
                }
                Cards[j].SetParticipant(participants[j]);

            }

            teams[i] = new ConfigurableTeam(baseTeams[i], Cards);
        }
    }
    BaseTeam[] CompileBaseTeams(){
        List<BaseTeam> baseTeams = new List<BaseTeam>();
        foreach(ConfigurableTeam team in teams){
            baseTeams.Add(team.GetTeam());
        }
        return baseTeams.ToArray();
    }
    public void StartGame(){
        InitializeGameStarter(CompileBaseTeams());
        
    }
    void InitializeGameStarter(BaseTeam[] baseTeams){

        GameStarter.StartGame(baseTeams);

        // hiding after the game has started
        Hide();
        ToggleRendering(false); // disabling the camera at the end just in case
    }
    public void SaveConfiguration(){
        BaseTeam[] CompiledTeams = CompileBaseTeams();
        string savefileName = GetSceneName();
        SaveSystem.SaveTeamConfigs(CompiledTeams, savefileName);
    }
    void Hide(){
        gameObject.SetActive(false);
    }
    void ToggleRendering(bool value){
        RenderCam.enabled = value;
        Canvas.SetActive(value);
    }
    string GetSceneName(){ // used for save file naming
        return SceneManager.GetActiveScene().name;
    }
}
