public class Joystick{
	public string name;
	public int position;
    public InputList input;

    public Joystick(){
        name = "";
        position = 98;
        input = new InputList(position);
    }
    public Joystick(string _name, int _position){ 
        name = _name;
        position = _position;

        input = new InputList(_position);
    }
}