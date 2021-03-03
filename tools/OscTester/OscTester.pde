SimBci simBci;
int simBciWidth, simBciNodeWidth, simBciHeight;

SimLed simLed;
int simLedCount = 240;
PImage simLedPreview;
int simLedPreviewScale = 1;
int simLedPreviewW = 1;
int simLedPreviewH = 1;

void setup() {
  size(640, 480, P2D);
  oscSetup();
  
  simBci = new SimBci();
  simBciWidth = width;
  simBciHeight = height/2;
  simBciNodeWidth = simBciWidth / simBci.nodes.length;
  
  simLed = new SimLed(simLedCount);
  int dim = int(sqrt(simLed.numLeds));
  simLedPreview = createImage(dim, dim, RGB);
  simLedPreviewW = simLedPreview.width / simLedPreviewScale;
  simLedPreviewH = simLedPreview.height / simLedPreviewScale;
  simLedPreview.loadPixels();
}

void draw() {
  background(0);
  
  simBci.update();
  simLed.update();
  
  for (int i=0; i<simBci.nodes.length; i++) {
    rect(simBciNodeWidth*i, simBciHeight, simBciNodeWidth, -simBci.nodes[i].normVal * simBciHeight);
  }
  
  for (int i=0; i<simLed.numLeds; i++) {
    int loc = i*3;
    int r = (int) simLed.buffer[loc];
    int g = (int) simLed.buffer[loc+1];
    int b = (int) simLed.buffer[loc+2];
    color col = color(r,g,b);

    if (i < simLedPreview.pixels.length) {
      simLedPreview.pixels[i] = col;
    } else {
      break;
    }
  }
  simLedPreview.updatePixels();
  image(simLedPreview, 0, height/2, simLedPreview.width*simLedPreviewW, simLedPreview.height*simLedPreviewH);
}
