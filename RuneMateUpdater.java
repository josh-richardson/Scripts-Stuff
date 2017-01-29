package com.eagles13.updater;

import java.io.*;
import java.net.URL;
import java.nio.channels.Channels;
import java.nio.channels.ReadableByteChannel;
import java.nio.charset.Charset;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.util.Arrays;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

/**
 * Created by Joshua on 09/06/2016.
 */
public class RuneMateUpdater {
    public static void main(String[] args) {
        try {
            System.out.println("Updater: getting current version info...");
            File versionFile = new File("rmversion");
            if (!versionFile.isDirectory() && versionFile.exists() && versionFile.canRead()) {
                BufferedReader br = new BufferedReader(new FileReader(versionFile));
                checkUpdate(getNumbers(br.lines().findFirst().orElse("0")));
            } else {
                System.out.println("Updater: no current version info found...");
                checkUpdate(0);
            }
        } catch (Exception ex) {
            ex.printStackTrace();
        }
    }


    private static void checkUpdate(int oldVersion) {
        try {
            System.out.println("Updater: checking for a new version");
            String rmWebpage = webRequest("https://www.runemate.com/download/");
            String downloadUrl = getBetween(rmWebpage, "var downloadLink = \"", "\";");
            System.out.println("Updater: " + downloadUrl);
            int newVersion = getNumbers(downloadUrl);
            if (newVersion > oldVersion) {
                System.out.println("Updater: current version is " + oldVersion + " newer version available " + newVersion + " - downloading new version...");
                URL website = new URL(downloadUrl + "/standalone/RuneMate.jar");
                ReadableByteChannel rbc = Channels.newChannel(website.openStream());
                FileOutputStream fos = new FileOutputStream("RuneMate.jar");
                fos.getChannel().transferFrom(rbc, 0, Long.MAX_VALUE);
                Files.write(Paths.get("rmversion"), Arrays.<CharSequence>asList(String.valueOf(newVersion)), Charset.forName("UTF-8"));
            } else {
                System.out.println("Updater: no update needed");
            }
        } catch (Exception ex) {
            ex.printStackTrace();
        }
        System.out.println("Updater: complete");
    }

    private static String webRequest(String url) {
        System.setProperty("http.agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.87 Safari/537.36\n");
        StringBuilder response = new StringBuilder();
        try {
            URL website = new URL(url);
            BufferedReader in = new BufferedReader(new InputStreamReader(website.openStream()));
            in.lines().forEach(response::append);
            in.close();
        } catch (IOException e) {
            e.printStackTrace();
        }
        return response.toString();
    }

    private static String getBetween(String haystack, String pre, String post) {
        Pattern pattern = Pattern.compile( Pattern.quote(pre) + "(.+?)" + Pattern.quote(post));
        Matcher matcher = pattern.matcher(haystack);
        if (matcher.find()) {
            return matcher.group(1);
        }
        return "No match could be found.";
    }

    private static int getNumbers(String input) {
        try {
            return Integer.parseInt(input.replaceAll("\\D+", ""));
        } catch (Exception ex) {
            return 0;
        }
    }

}
