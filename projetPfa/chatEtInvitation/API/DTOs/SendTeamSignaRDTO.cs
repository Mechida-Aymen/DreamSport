namespace chatEtInvitation.API.DTOs
{
    public class SendTeamSignaRDTO
    {
      public int  id {  get; set; }
      public string contenu { get; set; }
      public DateTime dateEnvoi { get; set; }
      public UserInfoSignalRDTO emetteur { get; set; }

      public string statut { get; set; }
      public int? teamId {get ; set; }
      public int chatTeamId { get; set; }

    }
}
