package proseeda.com.phonelistener;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.content.Context;
import android.telephony.PhoneStateListener;
import android.telephony.TelephonyManager;
import android.widget.Toast;

import java.io.IOException;
import java.net.Socket;
import org.json.JSONObject;
import java.io.OutputStreamWriter;
import java.nio.charset.StandardCharsets;
import java.util.Date;

public class MainActivity extends AppCompatActivity {
    private static final String serverAddress="18.224.148.94";
    private static final int serverPort=8099;
    private PhoneStateListener callStateListener;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        TelephonyManager telephonyManager =
                (TelephonyManager)getSystemService(Context.TELEPHONY_SERVICE);

        callStateListener = new PhoneStateListener() {
            //@todo support callswitch
            boolean callStarted = false;
            Date date;
            String incomingNumer1;
            public void onCallStateChanged(int state, String incomingNumber)
            {
                try{
                    if(state==TelephonyManager.CALL_STATE_RINGING) {
                        Toast.makeText(getApplicationContext(), "Phone Is Riging from: " + incomingNumber,
                                Toast.LENGTH_LONG).show();
                        Thread thread = new Thread(new SocketSendThread());
                        thread.start();
                        try {
                            thread.join();
                        } catch (InterruptedException ex) {
                            ex.printStackTrace();
                        }
                        //thread.stop();
                    }
                    if(state==TelephonyManager.CALL_STATE_OFFHOOK){
                        Toast.makeText(getApplicationContext(),"Phone is Currently in A call with: " + incomingNumber,
                                Toast.LENGTH_LONG).show();
                        callStarted=true;
                        date = new Date(System.currentTimeMillis());//capture call start
                        incomingNumer1=incomingNumber;
//                        SocketSendThread soc = new SocketSendThread();
//                        soc.number=incomingNumber;
//                        Thread thread = new Thread(new SocketSendThread());
//                        thread.start();
//                        try {
//                            thread.join();
//                        }catch(InterruptedException ex){
//                            ex.printStackTrace();
//                        }
                       // thread.stop();
                    }

                    if(state==TelephonyManager.CALL_STATE_IDLE) {
                        Toast.makeText(getApplicationContext(), "phone is neither ringing nor in a call",
                                Toast.LENGTH_LONG).show();
                        if(callStarted) {
                            callStarted = false;
                            SocketSendThread soc = new SocketSendThread();
                            soc.number=incomingNumer1;
                            soc.duration=System.currentTimeMillis()-date.getTime();
                            Thread thread = new Thread(soc);
                            thread.start();
                            try {
                                thread.join();
                            } catch (InterruptedException ex) {
                                ex.printStackTrace();
                            }
                        }
                        //thread.stop();

                    }
                }catch(Exception e){
                    e.printStackTrace();
                }
            }
        };
        telephonyManager.listen(callStateListener,PhoneStateListener.LISTEN_CALL_STATE);

    }
    class SocketSendThread implements Runnable {

        public String number;
        public long duration;
        public void run(){
            sendMessageToServer();
        }

        protected void sendMessageToServer() {

            Socket socket = null;
            long minutes = duration/60000;
            long hours = minutes/60;
            minutes = minutes-hours*60;
            StringBuffer sb = new StringBuffer(Long.toString(hours));
            sb.append(".");
            sb.append(Long.toString(minutes));
            try {
                JSONObject json = new JSONObject("{\"Name\": \"" + number + "\",\"Case\": \"zivcase\",\"Hour\": \"" +
                        sb.toString() +
                        "\", \"Description\": \" phone call from: " +number+ "\",\"user\": \"Ziv Yankowitz`\",\"Source\": \"phone call\"" +
                        "}");
//                Toast.makeText(getApplicationContext(), "going to send to server: " + json.toString(),
//                        Toast.LENGTH_LONG).show();
                Socket s = new Socket(serverAddress, serverPort);
                OutputStreamWriter out = new OutputStreamWriter(
                        s.getOutputStream(), StandardCharsets.UTF_8);
                out.write(json.toString());
                out.flush();


            } catch (Exception e) {
                // TODO Auto-generated catch block
                e.printStackTrace();
                // response = "UnknownHostException: " + e.toString();
            } finally {
                if (socket != null) {
                    try {
                        socket.close();
                    } catch (IOException e) {
                        // TODO Auto-generated catch block
                        e.printStackTrace();
                    }
                }
            }
            //return response;
        }
    }



}

