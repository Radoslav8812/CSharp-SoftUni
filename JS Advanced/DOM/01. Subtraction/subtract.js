function subtract() {

    let input1 = document.getElementById('firstNumber');
    let digits1 = Number(input1.value)
    let input2 = document.getElementById('secondNumber');
    let digits2 = Number(input2.value)
    let resultElement = document.getElementById('result');
    let result = digits1 - digits2;
    resultElement.textContent = result;;
}