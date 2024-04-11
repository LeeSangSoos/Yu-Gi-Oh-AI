dqn_agent: 
legal:  OrderedDict([(3179, None), (2322, None), (1900, None), (2533, None), (2111, None)])
raw:  ['endpage', 'summon-0-11', 'summon-0-9', 'summon-0-12', 'summon-0-10']
dqn_agent: 
legal:  OrderedDict([(3179, None)])
raw:  ['endpage']
dqn_agent: 
legal:  OrderedDict([(3149, None), (3179, None)])
raw:  ['endpage']

Logs saved in experiments/custom_dqn_result/
---------------------------------------------------------------------------
IndexError                                Traceback (most recent call last)
<ipython-input-7-858c52b3876e> in <cell line: 1>()
     16             logger.log_performance(
     17                 env.timestep,
---> 18                 tournament(
     19                     env,
     20                     1,

3 frames
/usr/local/lib/python3.10/dist-packages/rlcard/utils/utils.py in tournament(env, num)
    208     counter = 0
    209     while counter < num:
--> 210         _, _payoffs = env.run(is_training=False)
    211         if isinstance(_payoffs, list):
    212             for _p in _payoffs:

/usr/local/lib/python3.10/dist-packages/rlcard/envs/env.py in run(self, is_training)
    142             # Agent plays
    143             if not is_training:
--> 144                 action, _ = self.agents[player_id].eval_step(state)
    145             else:
    146                 action = self.agents[player_id].step(state)

/usr/local/lib/python3.10/dist-packages/rlcard/agents/dqn_agent.py in eval_step(self, state)
    176         print("legal: ", state['legal_actions'])
    177         print("raw: ", state['raw_legal_actions'])
--> 178         info['values'] = {state['raw_legal_actions'][i]: float(q_values[list(state['legal_actions'].keys())[i]]) for i in range(len(state['legal_actions']))}
    179 
    180         return best_action, info

/usr/local/lib/python3.10/dist-packages/rlcard/agents/dqn_agent.py in <dictcomp>(.0)
    176         print("legal: ", state['legal_actions'])
    177         print("raw: ", state['raw_legal_actions'])
--> 178         info['values'] = {state['raw_legal_actions'][i]: float(q_values[list(state['legal_actions'].keys())[i]]) for i in range(len(state['legal_actions']))}
    179 
    180         return best_action, info

IndexError: list index out of range