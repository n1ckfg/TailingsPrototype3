class SimLed {

  PImage buffer;
  int numLeds;

  SimLed(int _numLeds) {
    numLeds = _numLeds;
    int dim = int(sqrt((float) numLeds));
    buffer = createImage(dim, dim, RGB);
    buffer.loadPixels();
  }
  
  void update() {
    for (int i=0; i<buffer.pixels.length; i++) {
      buffer.pixels[i] = color(random(255),127,63);
    }
    buffer.updatePixels();
    
    OscMessage msg = new OscMessage("/simled");
    
    msg.add(convertImageToByteArray(buffer));
    
    oscP5.send(msg, myRemoteLocation);
  }
  
}
