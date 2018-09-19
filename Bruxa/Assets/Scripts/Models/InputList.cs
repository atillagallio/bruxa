
public class InputList
{
  public string Horizontal;
  public string Vertical;
  public string Jump;
  public string Fire1;
  public string Fire2;
  public string Fire3;
  public string Fire4;
  public string Start;

  public InputList(int pos)
  {
    Fire1 = "Fire1_P" + (pos + 1).ToString();
    Fire2 = "Fire2_P" + (pos + 1).ToString();
    Fire3 = "Fire3_P" + (pos + 1).ToString();
    Fire4 = "Fire4_P" + (pos + 1).ToString();
    Jump = "Jump_P" + (pos + 1).ToString();
    Start = "Start_P" + (pos + 1).ToString();
    Horizontal = "Horizontal_P" + (pos + 1).ToString();
    Vertical = "Vertical_P" + (pos + 1).ToString();

  }
}