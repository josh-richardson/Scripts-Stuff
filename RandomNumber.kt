
import java.util.*

/**
 * Created by Joshua on 20/10/2016.
 */

fun main(args : Array<String>) {
    //This commented single line is an alternative program - I thought it might be slightly risky to just submit it, so I figured I'd write a quick bubble sort too.
    Random().ints(1, 256).distinct().limit(6).sorted().forEach(::println)



    //As no max lotto number has been specified, I've assumed 256 here.
    val numbersArray = ArrayList<Int>()
    while (numbersArray.size < 6) {
        val nextInt = Random().nextInt(256)
        if (!numbersArray.contains(nextInt)) {
            numbersArray.add(nextInt)
        }
    }
    bubbleSort(numbersArray).forEach(::println)
}

fun bubbleSort(arr : ArrayList<Int>): ArrayList<Int> {
    var len = arr.size
    do {
        var newLen = 0
        for (i in 1..len-1) {
            if (arr[i - 1] > arr[i]) {
                newLen = i
                val temp = arr[i-1]
                arr[i-1] = arr[i]
                arr[i] = temp
            }
        }
        len = newLen
    } while (len != 0)
    return arr
}