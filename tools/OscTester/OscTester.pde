SimBci simBci;
int simBciWidth, simBciNodeWidth, simBciHeight;

SimLed simLed;
int simLedCount = 43711;

void setup() {
  size(640, 480, P2D);
  oscSetup();
  
  simBci = new SimBci();
  simBciWidth = width;
  simBciHeight = height/2;
  simBciNodeWidth = simBciWidth / simBci.nodes.length;
  
  simLed = new SimLed(15000);//43711);
}

void draw() {
  background(0);
  
  simBci.update();
  simLed.update();
  
  for (int i=0; i<simBci.nodes.length; i++) {
    rect(simBciNodeWidth*i, simBciHeight, simBciNodeWidth, -simBci.nodes[i].normVal * simBciHeight);
  }
  
  image(simLed.buffer, 0, height/2);
}
