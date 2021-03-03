class SimLed {

  byte[] buffer;
  int numLeds;
  float ledBrightness = 255;
  
  SimLed(int _numLeds) {
    numLeds = _numLeds;
    buffer = new byte[numLeds*3];
  }
  
  void update() {
    for (int i=0; i<buffer.length; i+=3) {
      color argb = color(random(255),127,63);
      int r = (argb >> 16) & 0xFF;  // Faster way of getting red(argb)
      int g = (argb >> 8) & 0xFF;   // Faster way of getting green(argb)
      int b = argb & 0xFF;          // Faster way of getting blue(argb)
      buffer[i] = byte(r);
      buffer[i+1] = byte(g);
      buffer[i+2] = byte(b);
    }
    
    OscMessage msg = new OscMessage("/simled");
    
    msg.add(buffer);
    
    oscP5.send(msg, myRemoteLocation);
  }
  
}
