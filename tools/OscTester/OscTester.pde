SimBci simBci;
SimLed simLed;

int simBciWidth, simBciNodeWidth, simBciHeight;


void setup() {
  size(640, 480, P2D);
  oscSetup();
  
  simBci = new SimBci();
  simBciWidth = width;
  simBciHeight = height/2;
  simBciNodeWidth = simBciWidth / simBci.nodes.length;
}

void draw() {
  background(0);
  
  simBci.update();
  
  for (int i=0; i<simBci.nodes.length; i++) {
    rect(simBciNodeWidth*i, simBciHeight, simBciNodeWidth, -simBci.nodes[i].normVal * simBciHeight);
  }
 
}
