import oscP5.*;
import netP5.*;

String ipNumber = "127.0.0.1";
int receivePort = 9001;
int sendPort = 9000;
int datagramSize = 1024000;
OscP5 oscP5;
NetAddress myRemoteLocation;

String[] oscChannelNames = { "isadora/1","isadora/2","isadora/3" };
float[] oscReceiveData = { 0,0,0 };
float[] oscSendData = { 0 };

void oscSetup() {
  OscProperties oscProperties = new OscProperties();
  oscProperties.setListeningPort(receivePort);
  oscProperties.setDatagramSize(datagramSize);
  oscP5 = new OscP5(this, oscProperties);
  myRemoteLocation = new NetAddress(ipNumber, sendPort);
}

void oscEvent(OscMessage myMessage) {
  println(myMessage);
  for (int i=0;i<oscChannelNames.length;i++) {
    if (myMessage.checkAddrPattern("/" + oscChannelNames[i])) {
      if (myMessage.checkTypetag("f")) {  // types are i = int, f = float, s = String, ifs = all
        oscReceiveData[i] = myMessage.get(0).floatValue();  // commands are intValue, floatValue, stringValue
      }  
    }
  }
}

void oscSend() {
  OscMessage myMessage;
  
  for (int i=0; i<oscSendData.length; i++) {
    myMessage = new OscMessage("/" + oscChannelNames[i]);
    myMessage.add(oscSendData[i]);
    oscP5.send(myMessage, myRemoteLocation);
  }
}
