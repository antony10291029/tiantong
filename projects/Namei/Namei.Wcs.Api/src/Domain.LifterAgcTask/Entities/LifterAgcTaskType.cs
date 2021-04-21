using Midos.Domain;

namespace Namei.Wcs.Aggregates
{
  public class LifterAgcTaskType: IEntity
  {
    public long Id { get; private set; }

    public string Key { get; private set; }

    public string Name { get; private set; }

    public string WebHook { get; private set; }

    public LifterAgcTaskType(
      string key,
      string name,
      string webHook
    ) {
      Key = key;
      Name = name;
      WebHook = webHook;
    }

    public void Update(LifterAgcTaskTypeController.UpdateParams param)
    {
      Key = param.Key;
      Name = param.Name;
      WebHook = param.WebHook;
    }
  }
}
