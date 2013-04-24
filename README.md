Kamisado
========

An implementation of a Kamisado playing agent. The agent is implemented
using the minimax algorithm with alpha-beta pruning. This implementation
was used in our (me and @pskepp) bachelor thesis.

Due to the time constraints of the project, the code ended up a bit messy.
Instead of reading the code to understand how the bot works, it's probably
easier to read the report.

Report
========

Abstract
--------

Kamisado is a two player board game. This report examines the question
of how to construct a competent Kamisado playing agent.

Kamisado can be played in single round matches, or in matches
consisting of several rounds. The single round match was searched exhaustively.
The result shows that the starting player can force a win in
a single round match regardless of the strategy of the opponent.

The longer matches were too complex to be searched exhaustively.
Therefore, a set of heuristic Kamisado strategies were developed and
agents using different combinations of the strategies were implemented.

The different agents were compared by playing matches between
them. Agents using a specific combination of the heuristic strategies
tended to perform better than other agents. This indicates that this
combination of the heuristic strategies creates a more competent agent
than other combinations.

The first draft of the report is available here
http://www.csc.kth.se/utbildning/kth/kurser/DD143X/dkand13/Group4Per/report/17-setterquist-skeppstedt.pdf
